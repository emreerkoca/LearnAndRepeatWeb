using System;

namespace LearnAndRepeatWeb.Infrastructure.Entities.User
{
    public class UserModel : IBaseEntity, ISoftDeletableEntity, IUpdatableEntity
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeleteDate { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
