using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Requests.User;
using LearnAndRepeatWeb.Contracts.Responses.User;
using LearnAndRepeatWeb.Infrastructure.AppDbContext;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public PostTokenResponse PostToken(PostTokenRequest postTokenRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<PostUserResponse> PostUser(PostUserRequest postUserRequest)
        {
            bool isUserExist = _appDbContext.User.Any(m => m.Email == postUserRequest.Email);

            if (isUserExist)
            {
                throw new Exception("Conflict Exception!"); //TODO: add custom exceptions 
            } //TODO: add MessageResource.resx file for error messages

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


            //TODO: retunn mapping + response
            return new PostUserResponse();
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
