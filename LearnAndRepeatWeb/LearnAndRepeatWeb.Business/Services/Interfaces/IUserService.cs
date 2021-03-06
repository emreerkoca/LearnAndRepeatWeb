using LearnAndRepeatWeb.Contracts.Requests.User;
using LearnAndRepeatWeb.Contracts.Responses.User;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> PostUser(PostUserRequest postUserRequest);
        Task PatchUser(string userKey, PatchUserRequest patchUserRequest);
        Task<UserResponse> GetUser(string userKey);
        Task PutUserAsConfirmed(long userId, string tokenValue);
        Task<AuthenticationTokenResponse> PostAuthenticationToken(PostAuthenticationTokenRequest postAuthenticationTokenRequest);
        Task<UserTokenResponse> PostUserToken(long userId, UserTokenType userTokenType);

    }
}
