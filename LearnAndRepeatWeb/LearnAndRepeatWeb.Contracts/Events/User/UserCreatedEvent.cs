using LearnAndRepeatWeb.Contracts.Responses.User;

namespace LearnAndRepeatWeb.Contracts.Events.User
{
    public class UserCreatedEvent
    {
        public UserResponse UserResponse { get; set; }
    }
}
