using System;

namespace LearnAndRepeatWeb.Infrastructure.Entities.User
{
    public class UserModel : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
