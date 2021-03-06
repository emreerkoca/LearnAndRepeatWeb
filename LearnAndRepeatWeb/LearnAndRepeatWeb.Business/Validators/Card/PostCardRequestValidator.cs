using FluentValidation;
using LearnAndRepeatWeb.Contracts.Requests.Card;

namespace LearnAndRepeatWeb.Business.Validators.Card
{
    public class PostCardRequestValidator : AbstractValidatorCustomized<PostCardRequest>
    {
        public PostCardRequestValidator()
        {
            RuleFor(r => r.Content)
                .NotEmpty();

            RuleFor(r => r.Header)
                .NotEmpty();
        }
    }
}
