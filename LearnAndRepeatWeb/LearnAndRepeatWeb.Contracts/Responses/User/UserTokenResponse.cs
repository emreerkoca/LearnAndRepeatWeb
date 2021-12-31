using LearnAndRepeatWeb.Contracts.Enums.User;
using System;

namespace LearnAndRepeatWeb.Contracts.Responses.User
{
    public class UserTokenResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public UserTokenType UserTokenType { get; set; }
        public string TokenValue { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
