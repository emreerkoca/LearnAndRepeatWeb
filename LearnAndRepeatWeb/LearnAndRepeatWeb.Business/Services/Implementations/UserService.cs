using AutoMapper;
using LearnAndRepeatWeb.Business.ConfigModels;
using LearnAndRepeatWeb.Business.CustomExceptions;
using LearnAndRepeatWeb.Business.Resources;
using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Requests.User;
using LearnAndRepeatWeb.Contracts.Responses.User;
using LearnAndRepeatWeb.Infrastructure.AppDbContext;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly UserConfigSectionModel _userConfigSectionModel;

        public UserService(AppDbContext appDbContext, IMapper mapper, IOptions<UserConfigSectionModel> userConfigSectionModelOptions)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _userConfigSectionModel = userConfigSectionModelOptions.Value;
        }

        public PostTokenResponse PostToken(PostTokenRequest postTokenRequest)
        {
            var userModel = _appDbContext.User.FirstOrDefault(m => m.Email.Equals(postTokenRequest.Email));

            if (userModel == null)
            {
                throw new ValidationException(Resource.InvalidUsernameOrPassword);
            }

            var hashedPassword = GetSaltedAndHashedPassword(userModel.Salt, postTokenRequest.Password);

            if (!userModel.Password.Equals(hashedPassword))
            {
                throw new ValidationException(Resource.InvalidUsernameOrPassword);
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_userConfigSectionModel.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, userModel.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(2 ),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new PostTokenResponse
            {
                AccountId = userModel.Id,
                Token = tokenHandler.WriteToken(securityToken)
            };
        }

        public async Task<PostUserResponse> PostUser(PostUserRequest postUserRequest)
        {
            bool isUserExist = _appDbContext.User.Any(m => m.Email == postUserRequest.Email);

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

            await _appDbContext.User.AddAsync(userModel);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<PostUserResponse>(userModel);            
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
    }
}
