using FluentValidation;

namespace Application.Sections.Commands;

public class CreateSectionCommandValidator : AbstractValidator<CreateSectionCommand>
{
    public CreateSectionCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);
        
        RuleFor(x => x.UserId).NotEmpty();
    }
}