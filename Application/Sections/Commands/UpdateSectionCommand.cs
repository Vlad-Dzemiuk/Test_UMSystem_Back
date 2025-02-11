using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Exceptions;
using Domain.Sections;
using Domain.Users;
using MediatR;

namespace Application.Sections.Commands;

public class UpdateSectionCommand : IRequest<Result<Section, SectionException>>
{
    public required Guid SectionId { get; init; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}

public class UpdateSectionCommandHandler(
    ISectionRepository sectionRepository,
    IUserRepository userRepository)
    : IRequestHandler<UpdateSectionCommand, Result<Section, SectionException>>
{
    public async Task<Result<Section, SectionException>> Handle(
        UpdateSectionCommand request,
        CancellationToken cancellationToken)
    {
        var sectionId = new SectionId(request.SectionId);
        var userId = new UserId(request.UserId);
        
        var section = await sectionRepository.GetById(request.SectionId.ToString());
        if (section == null)
        {
            return new SectionNotFoundException(sectionId);
        }
        
        if (section.UserId != userId)
        {
            return new SectionForUserNotFoundException(userId);
        }
        
        section.UpdateName(request.Name);
        await sectionRepository.Update(sectionId.ToString(), section);
        
        return section;
    }
}