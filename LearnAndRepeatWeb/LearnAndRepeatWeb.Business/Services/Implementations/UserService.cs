using AutoMapper;
using LearnAndRepeatWeb.Business.ConfigModels;
using LearnAndRepeatWeb.Business.CustomExceptions;
using LearnAndRepeatWeb.Business.Resources;
using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Requests.User;
using LearnAndRepeatWeb.Contracts.Responses.User;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using LearnAndRepeatWeb.Infrastructure.Repositories.User;
//using MassTransit;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IMapper _mapper;
        private readonly UserConfigSectionModel _userConfigSectionModel;
        //private readonly IBusControl _busControl;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public UserService(IUserRepository userRepository, IUserTokenRepository userTokenRepository, IMapper mapper, IOptions<UserConfigSectionModel> userConfigSectionModelOptions, /*IBusControl busControl,*/ IUserAuthorizationService userAuthorizationService)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _mapper = mapper;
            _userConfigSectionModel = userConfigSectionModelOptions.Value;
            //_busControl = busControl;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task<UserResponse> PostUser(PostUserRequest postUserRequest)
        {
            bool isUserExist = await _userRepository.Any(m => m.Email == postUserRequest.Email);

            if (isUserExist)
            {
                throw new ConflictException(string.Format(Resource.ConflictExceptionMessage, postUserRequest.Email));
            }

            #region Generate Hashed Password
            var saltByte = GenerateSaltByte();
            string hashedPassword = GenerateHashedPassword(saltByte, postUserRequest.Password);
            string salt = Convert.ToBase64String(saltByte);
            #endregion

            var userModel = new UserModel
            {
                Email = postUserRequest.Email,
                FirstName = postUserRequest.FirstName,
                LastName = postUserRequest.LastName,
                Password = hashedPassword,
                Salt = salt,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _userRepository.Add(userModel);

            UserResponse userResponse = _mapper.Map<UserResponse>(userModel);

            //await _busControl.Publish(new UserCreatedEvent
            //{
            //    UserResponse = userResponse
            //});

            return userResponse;
        }

        public async Task PatchUser(string userKey, PatchUserRequest patchUserRequest)
        {
            _userAuthorizationService.CheckUserPermission(userKey);
            long userId = _userAuthorizationService.ConvertUserKeyToUserResponse(userKey).Id;

            var userModel = await _userRepository.GetById(userId);

            if (userModel == null)
            {
                throw new NotFoundException(Resource.UserCouldNotFound);
            }

            if (!string.IsNullOrEmpty(patchUserRequest.FirstName))
            {
                userModel.FirstName = patchUserRequest.FirstName;
            }

            if (!string.IsNullOrEmpty(patchUserRequest.LastName))
            {
                userModel.LastName = patchUserRequest.LastName;
            }

            userModel.UpdateDate = DateTime.UtcNow;

            await _userRepository.Update(userModel);
        }

        public async Task<UserResponse> GetUser(string userKey)
        {
            _userAuthorizationService.CheckUserPermission(userKey);
            long userId = _userAuthorizationService.ConvertUserKeyToUserResponse(userKey).Id;

            var userModel = await _userRepository.GetById(userId);

            if (userModel == null)
            {
                throw new NotFoundException(Resource.UserCouldNotFound);
            }

            return _mapper.Map<UserResponse>(userModel);
        }

        public async Task PutUserAsConfirmed(long userId, string tokenValue)
        {
            var userModel = await GetUserModel(userId);            
            var tokenModel = await _userTokenRepository.GetItem(m => m.UserId == userId && m.TokenValue == tokenValue);

            if (tokenModel == null)
            {
                throw new NotFoundException(Resource.UserTokenCouldNotFound);
            }

            if (tokenModel.IsUsed)
            {
                throw new ValidationException(Resource.TokenIsAlreadyUsed);
            }

            userModel.IsEmailConfirmed = true;
            userModel.UpdateDate = DateTime.UtcNow;

            await _userRepository.Update(userModel);

            tokenModel.IsUsed = true;

            await _userTokenRepository.Update(tokenModel);
            
            //await _busControl.Publish(new UserConfirmedEvent
            //{
            //    UserId = userId
            //});
        }

        public async Task<AuthenticationTokenResponse> PostAuthenticationToken(PostAuthenticationTokenRequest postAuthenticationTokenRequest)
        {
            var userModel = await _userRepository.GetItem(m => m.Email.Equals(postAuthenticationTokenRequest.Email));

            if (userModel == null)
            {
                throw new ValidationException(Resource.InvalidUsernameOrPassword);
            }

            if (!userModel.IsEmailConfirmed)
            {
                throw new ValidationException(Resource.UserEmailIsNotConfirmed);
            }

            var hashedPassword = GetSaltedAndHashedPassword(userModel.Salt, postAuthenticationTokenRequest.Password);

            if (!userModel.Password.Equals(hashedPassword))
            {
                throw new ValidationException(Resource.InvalidUsernameOrPassword);
            }

            UserResponse userResponse = _mapper.Map<UserResponse>(userModel);
            var userKey = _userAuthorizationService.ConvertUserResponseToUserKey(userResponse);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_userConfigSectionModel.AuthenticationSecret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, userKey)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);


            return new AuthenticationTokenResponse
            {
                UserKey = userKey,
                Token = tokenHandler.WriteToken(securityToken)
            };
        }

        public async Task<UserTokenResponse> PostUserToken(long userId, UserTokenType userTokenType)
        {
            await GetUserModel(userId);

            UserTokenModel userTokenModel = new UserTokenModel
            {
                UserId = userId,
                UserTokenType = userTokenType,
                TokenValue = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                CreateDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddDays(2)
            };

            await _userTokenRepository.Add(userTokenModel);

            UserTokenResponse userTokenResponse = _mapper.Map<UserTokenResponse>(userTokenModel);

            //
            //await _busControl.Publish(new UserTokenCreatedEvent
            //{
            //    UserTokenResponse = userTokenResponse
            //});

            return userTokenResponse;
        }

        private byte[] GenerateSaltByte()
        {
            byte[] saltByte = new byte[128 / 8];

            using (var rndNumberGenerator = RandomNumberGenerator.Create())
            {
                rndNumberGenerator.GetBytes(saltByte);
            }

            return saltByte;
        }

        private string GenerateHashedPassword(byte[] saltByte, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltByte,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        private string GetSaltedAndHashedPassword(string salt, string password)
        {
            return GenerateHashedPassword(Convert.FromBase64String(salt), password);
        }

        private async Task<UserModel> GetUserModel(long userId)
        {
            var userModel = await _userRepository.GetById(userId);

            if (userModel == null)
            {
                throw new NotFoundException(Resource.UserCouldNotFound);
            }

            return userModel;
        }

        
    }
}
