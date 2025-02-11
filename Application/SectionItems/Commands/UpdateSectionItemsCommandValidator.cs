using FluentValidation;

namespace Application.SectionItems.Commands;

public class UpdateSectionItemsCommandValidator : AbstractValidator<UpdateSectionItemsCommand>
{
    public UpdateSectionItemsCommandValidator()
    {
        RuleFor(x => x.SectionItemId).NotEmpty();

        RuleFor(x => x.Title).NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);

        RuleFor(x => x.Content).NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);
    }
}