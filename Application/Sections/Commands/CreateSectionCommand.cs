using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Exceptions;
using Domain;
using MediatR;

namespace Application.Sections.Commands;

public class CreateSectionCommand : IRequest<Result<Section, SectionException>>
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}

/*public class CreateSectionCommandHandler(ISectionRepository sectionRepository) 
    : IRequestHandler<CreateSectionCommand, Result<Section, SectionException>>
{
    public async Task<Result<Section, SectionException>> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var sectionId = SectionId.New();

        var section = Section.New(sectionId, userId, request.Name);

        try
        {
            await sectionRepository.Create(section);
            return section;
        }
        catch (Exception ex)
        {
            return new SectionUnknownException(sectionId, ex);
        }
    }
}*/
