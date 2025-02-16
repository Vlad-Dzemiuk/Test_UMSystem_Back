using FluentValidation;

namespace Application.SectionItems.Commands;

public class UpdateSectionItemsCommandValidator : AbstractValidator<UpdateSectionItemsCommand>
{
    public UpdateSectionItemsCommandValidator()
    {
        RuleFor(x => x.SectionItemId)
            .NotEmpty().WithMessage("Section item ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(50).WithMessage("Title must not exceed 50 characters.");
        
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(50).WithMessage("Content must not exceed 50 characters.");

        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("Section ID is required.");
    }
}