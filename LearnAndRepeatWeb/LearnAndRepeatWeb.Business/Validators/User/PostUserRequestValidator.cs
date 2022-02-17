using FluentValidation;
using LearnAndRepeatWeb.Contracts.Requests.User;

namespace LearnAndRepeatWeb.Business.Validators.User
{
    public class PostUserRequestValidator : AbstractValidatorCustomized<PostUserRequest>
    {
        public PostUserRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty();

            RuleFor(r => r.LastName)
                .NotEmpty();

            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(r => r.Password)
                .NotEmpty()
                .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$");

            RuleFor(r => r.PasswordConfirmation)
                .NotEmpty();

            RuleFor(r => r.Password)
                .Equal(r => r.PasswordConfirmation);
        }
    }
}
