using FluentValidation;

namespace Application.SectionItems.Commands;

public class CreateSectionItemCommandValidator : AbstractValidator<CreateSectionItemsCommand>
{
    public CreateSectionItemCommandValidator()
    {
        RuleFor(x => x.SectionId).NotEmpty();
    }
}