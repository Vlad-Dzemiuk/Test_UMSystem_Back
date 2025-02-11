using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Exceptions;
using Domain.Sections;
using Domain.Users;
using MediatR;

namespace Application.Sections.Commands;

public class CreateSectionCommand : IRequest<Result<Section, SectionException>>
{
    public required Guid SectionId { get; init; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}

public class CreateSectionCommandHandler(
    ISectionRepository sectionRepository,
    IUserRepository userRepository)
    : IRequestHandler<CreateSectionCommand, Result<Section, SectionException>>
{
    public async Task<Result<Section, SectionException>> Handle(
        CreateSectionCommand request,
        CancellationToken cancellationToken)
    {
        var sectionId = new SectionId(request.SectionId);
        var userId = new UserId(request.UserId);
        
        var existingSection = await sectionRepository.GetById(request.SectionId.ToString());
        if (existingSection != null)
        {
            return new SectionAlreadyExistsException(existingSection.Id);
        }
        
        var user = await userRepository.GetById(request.UserId.ToString());
        if (user == null)
        {
            return new SectionForUserNotFoundException(userId);
        }
        
        return await CreateEntity(sectionId, userId, request.Name);
    }

    private async Task<Result<Section, SectionException>> CreateEntity(
        SectionId sectionId,
        UserId userId,
        string name)
    {
        try
        {
            var section = Section.New(sectionId, userId, name);
            await sectionRepository.Create(section);
            return section;
        }
        catch (Exception ex)
        {
            return new SectionUnknownException(sectionId, ex);
        }
    }
}
