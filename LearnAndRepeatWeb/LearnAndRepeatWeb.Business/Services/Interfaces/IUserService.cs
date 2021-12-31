using LearnAndRepeatWeb.Contracts.Requests.User;
using LearnAndRepeatWeb.Contracts.Responses.User;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<PostUserResponse> PostUser(PostUserRequest postUserRequest);
        Task PutUserAsConfirmed(long userId, string tokenValue);
        PostTokenResponse PostToken(PostTokenRequest postTokenRequest);
        Task<UserTokenResponse> PostUserToken(long userId, UserTokenType userTokenType);

    }
}
