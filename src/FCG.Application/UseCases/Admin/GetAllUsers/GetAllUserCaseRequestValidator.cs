using FluentValidation;

namespace FCG.Application.UseCases.Admin.GetAllUsers
{
    public class GetAllUserCaseRequestValidator : AbstractValidator<GetAllUserCaseRequest>
    {
        public GetAllUserCaseRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Name))
                .WithMessage("Name must not exceed 255 characters");

            RuleFor(x => x.Email)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must not exceed 255 characters")
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must be a valid email address");
        }
    }
}
