using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Requests.User;
using LearnAndRepeatWeb.Contracts.Responses.User;
using LearnAndRepeatWeb.Infrastructure.AppDbContext;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using System;
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
            //validations

            var userModel = new UserModel
            {
                Email = postUserRequest.Email,
                FirstName = postUserRequest.FirstName,
                LastName = postUserRequest.LastName,
                Password = postUserRequest.Password, //hash password
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _appDbContext.User.AddAsync(userModel);
            await _appDbContext.SaveChangesAsync();


            //retunn mapping + response
            return new PostUserResponse();
        }
    }
}
