using LearnAndRepeatWeb.Contracts.Requests.User;
using LearnAndRepeatWeb.Contracts.Responses.User;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<PostUserResponse> PostUser(PostUserRequest postUserRequest);
        PostTokenResponse PostToken(PostTokenRequest postTokenRequest);
    }
}
