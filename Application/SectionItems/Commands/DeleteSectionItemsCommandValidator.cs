using FluentValidation;

namespace Application.SectionItems.Commands;

public class DeleteSectionItemsCommandValidator: AbstractValidator<DeleteSectionItemsCommand>
{
    public DeleteSectionItemsCommandValidator()
    {
        RuleFor(x => x.SectionItemId).NotEmpty();
    }
}