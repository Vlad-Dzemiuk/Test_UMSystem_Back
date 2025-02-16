using FluentValidation;

namespace Application.SectionItems.Commands;

public class CreateSectionItemCommandValidator : AbstractValidator<CreateSectionItemsCommand>
{
    public CreateSectionItemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);
        
        RuleFor(x => x.Content).NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);
        
        RuleFor(x => x.SectionId).NotEmpty();
    }
}