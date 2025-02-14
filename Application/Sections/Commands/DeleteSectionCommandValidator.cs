using FluentValidation;

namespace Application.Sections.Commands;

public class DeleteSectionCommandValidator : AbstractValidator<DeleteSectionCommand>
{
    public DeleteSectionCommandValidator()
    {
        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("Section ID is required.");
    }
}