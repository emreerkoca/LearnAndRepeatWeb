using FluentValidation;
using LearnAndRepeatWeb.Contracts.Requests.Card;

namespace LearnAndRepeatWeb.Business.Validators.Card
{
    public class PatchCardRequestValidator : AbstractValidatorCustomized<PatchCardRequest>
    {
        public PatchCardRequestValidator()
        {
            RuleFor(r => r.Id)
                .GreaterThan(0);
        }
    }
}
