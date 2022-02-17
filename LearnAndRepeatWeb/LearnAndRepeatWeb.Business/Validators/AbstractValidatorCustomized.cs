using FluentValidation;
using FluentValidation.Results;

namespace LearnAndRepeatWeb.Business.Validators
{
    public class AbstractValidatorCustomized<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                string validationErrors = string.Empty;

                foreach (var validationFailure in validationResult.Errors)
                {
                    validationErrors += "-" + validationFailure.ErrorMessage;
                }
                
                throw new CustomExceptions.ValidationException(validationErrors);
            }

            return validationResult;
        }
    }
}
