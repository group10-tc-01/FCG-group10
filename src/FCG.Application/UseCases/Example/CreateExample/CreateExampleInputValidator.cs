using FluentValidation;

namespace FCG.Application.UseCases.Example.CreateExample
{
    public class CreateExampleInputValidator : AbstractValidator<CreateExampleInput>
    {
        public CreateExampleInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters");
        }
    }
}