using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Exceptions;
using Domain.Section_Items;
using Domain.Sections;
using Domain.Users;
using MediatR;

namespace Application.SectionItems.Commands;

public class UpdateSectionItemsCommand: IRequest<Result<SectionItem, SectionItemException>>
{
    public required Guid SectionItemId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required Guid SectionId { get; init; }
}

/*public class UpdateSectionItemsCommandHandler(
    ISectionItemRepository sectionItemRepository,
    ISectionRepository sectionRepository)
    : IRequestHandler<UpdateSectionItemsCommand, Result<SectionItem, SectionItemException>>
{
    public async Task<Result<SectionItem, SectionItemException>> Handle(
        UpdateSectionItemsCommand request,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetById(new SectionId(request.SectionId));

        return await section.Match(
            async s =>
            {
                var sectionItem = await sectionItemRepository.GetById(new SectionItemId(request.SectionItemId));
                return await sectionItem.Match(
                    i => UpdateEntity(i, request.Title, request.Content, s),
                    () => Task.FromResult<Result<SectionItem, SectionItemException>>(
                        new SectionItemNotFoundException(new SectionItemId(request.SectionItemId())))
            },
            () => Task.FromResult<Result<SectionItem, SectionItemException>>(
                new GenderNotFoundException(new SectionId(request.SectionId))));
    }

    private async Task<Result<SectionItem, SectionItemException>> UpdateEntity(SectionItem sectionItem, string title, string content, Section section)
    {
        try
        {
            sectionItem.UpdateDetails(title, content, section.Id);

            return await sectionItemRepository.Update(sectionItem);
        }
        catch (Exception exception)
        {
            return new SectionItemUnknownException(sectionItem.Id, exception);
        }
    }
}*/