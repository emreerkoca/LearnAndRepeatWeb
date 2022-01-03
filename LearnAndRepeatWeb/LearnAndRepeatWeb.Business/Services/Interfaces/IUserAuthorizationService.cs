using LearnAndRepeatWeb.Contracts.Responses.User;

namespace LearnAndRepeatWeb.Business.Services.Interfaces
{
    public interface IUserAuthorizationService
    {
        public string ConvertUserResponseToUserKey(UserResponse userResponse);
        public UserResponse ConvertUserKeyToUserResponse(string userKey);
        public void CheckUserPermission(string userKey);
    }
}
