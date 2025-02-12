using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Exceptions;
using Domain;
using MediatR;

namespace Application.SectionItems.Commands;

public class DeleteSectionItemsCommand : IRequest<Result<SectionItem, SectionItemException>>
{
    public required Guid SectionItemId { get; set; }
}

public class DeleteSectionItemsCommandHandler(
    ISectionItemRepository sectionItemRepository) : IRequestHandler<DeleteSectionItemsCommand, Result<SectionItem, SectionItemException>>
{
    /*public async Task<Result<SectionItem, SectionItemException>> Handle(
        DeleteSectionItemsCommand request)
    {
        var sectionItem = await sectionItemRepository.GetById(new SectionItemId(request.SectionItemId));
        return await sectionItem.Match(
            s => DeleteEntity(s),
            () => Task.FromResult<Result<SectionItem, SectionItemException>>(
                new SectionItemNotFoundException(new SectionItemId(request.SectionItemId))));
    }*/

    /*private async Task<Result<SectionItem, SectionItemException>> DeleteEntity(
        SectionItem sectionItem)
    {
        try
        {
            return await sectionItemRepository.Delete(sectionItem);
        }
        catch (Exception exception)
        {
            return new SectionItemUnknownException(sectionItem.Id, exception);
        }
    }*/

    public Task<Result<SectionItem, SectionItemException>> Handle(DeleteSectionItemsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}