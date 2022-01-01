using System;

namespace LearnAndRepeatWeb.Infrastructure.Entities.User
{
    public class UserTokenModel : IBaseEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public UserTokenType UserTokenType { get; set; }
        public string TokenValue { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public enum UserTokenType
    {
        UserConfirmationToken = 1,
        PasswordResetToken = 2
    }
}
