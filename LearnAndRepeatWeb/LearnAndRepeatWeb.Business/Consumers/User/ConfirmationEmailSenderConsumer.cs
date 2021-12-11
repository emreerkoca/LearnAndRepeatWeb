using LearnAndRepeatWeb.Contracts.Events.User;
using MassTransit;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Consumers.User
{
    public class ConfirmationEmailSenderConsumer : IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            //TODO: Send confirmation email to user
        }
    }
}
