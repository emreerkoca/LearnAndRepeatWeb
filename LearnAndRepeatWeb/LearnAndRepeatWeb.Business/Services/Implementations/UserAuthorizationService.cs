using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Responses.User;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Text;

namespace LearnAndRepeatWeb.Business.Services.Implementations
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserResponse ConvertUserKeyToUserResponse(string userKey)
        {
            var byteArrayUserResponse = Convert.FromBase64String(userKey);
            var serializedUserResponse = Encoding.UTF8.GetString(byteArrayUserResponse);
            
            return JsonConvert.DeserializeObject<UserResponse>(serializedUserResponse);
        }

        public string ConvertUserResponseToUserKey(UserResponse userResponse)
        {
            string serializedObject = JsonConvert.SerializeObject(userResponse);
            var byteArrayObject = Encoding.UTF8.GetBytes(serializedObject);

            return Convert.ToBase64String(byteArrayObject);
        }

        public void CheckUserPermission(string userKey)
        {
            Claim claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name);

            if (claim == null)
            {
                throw new Exception(Resources.Resource.UserHasNoPermission);
            }

            var userKeyFromClaim = claim.Value;

            if (userKeyFromClaim != userKey)
            {
                throw new Exception(Resources.Resource.UserHasNoPermission);
            }
        }

    }
}
