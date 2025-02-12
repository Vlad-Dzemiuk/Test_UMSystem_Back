using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Exceptions;
using Domain;
using MediatR;

namespace Application.SectionItems.Commands;

public class CreateSectionItemsCommand : IRequest<Result<SectionItem, SectionItemException>>
{
    public required Guid SectionId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
}

/*public class CreateSectionItemsCommandHandler(
    ISectionItemRepository sectionItemRepository,
    ISectionRepository sectionRepository)
    : IRequestHandler<CreateSectionItemsCommand, Result<SectionItem, SectionItemException>>
{
    public async Task<Result<SectionItem, SectionItemException>> Handle(
        CreateSectionItemsCommand request,
        CancellationToken cancellationToken)
    {
        var sectionId = new SectionId(request.SectionId);
        
        var existingSectionItem = await sectionItemRepository.GetById(request.SectionId.ToString());

        if (existingSectionItem != null)
        {
            return new SectionItemAlreadyExistsException(existingSectionItem.Id);
        }

        return await CreateEntity(sectionId, request, cancellationToken);
    }

    private async Task<Result<SectionItem, SectionItemException>> CreateEntity(
        SectionId sectionId,
        CreateSectionItemsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = SectionItem.New(SectionItemId.New(), sectionId, request.Title, request.Content);
            await sectionItemRepository.Create(entity);
            return entity;
        }
        catch (Exception exception)
        {
            return new SectionItemUnknownException(SectionItemId.Empty(), exception);
        }
    }
}*/