using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Events.User;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using MassTransit;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Consumers.User
{
    public class UserConfirmationTokenCreatorConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IUserService _userService;
        public UserConfirmationTokenCreatorConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userResponse = context.Message.UserResponse;

            await _userService.PostUserToken(userResponse.Id, UserTokenType.UserConfirmationToken);
        }
    }
}
