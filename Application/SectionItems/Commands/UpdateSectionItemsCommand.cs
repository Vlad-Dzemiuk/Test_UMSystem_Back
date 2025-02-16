using API.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Exceptions;
using MediatR;

namespace Application.SectionItems.Commands;

public class UpdateSectionItemsCommand: IRequest<SectionItemDto>
{
    public string SectionItemId { get; }
    public string Title { get; }
    public string Content { get; }
    public string SectionId { get; }

    public UpdateSectionItemsCommand(string sectionItemId, UpdateSectionItemDto dto)
    {
        SectionItemId = sectionItemId;
        Title = dto.Title;
        Content = dto.Content;
        SectionId = dto.SectionId;
    }
}

public class UpdateSectionItemsCommandHandler : IRequestHandler<UpdateSectionItemsCommand, SectionItemDto>
{
    private readonly ISectionItemRepository _sectionItemRepository;
    private readonly ISectionItemQueries _sectionItemQueries;
    private readonly ISectionRepository _sectionRepository;
    private readonly ISectionQueries _sectionQueries;

    public UpdateSectionItemsCommandHandler(ISectionItemRepository sectionItemRepository, ISectionItemQueries sectionItemQueries, ISectionRepository sectionRepository, ISectionQueries sectionQueries)
    {
        _sectionItemRepository = sectionItemRepository;
        _sectionItemQueries = sectionItemQueries;
        _sectionRepository = sectionRepository;
        _sectionQueries = sectionQueries;
    }

    public async Task<SectionItemDto> Handle(UpdateSectionItemsCommand request, CancellationToken cancellationToken)
    {
        var existingSectionItem = await _sectionItemQueries.GetById(request.SectionItemId) 
                              ?? throw new SectionItemNotFoundException(request.SectionItemId);

        var oldSection = await _sectionQueries.GetById(existingSectionItem.SectionId);
        var newSection = await _sectionQueries.GetById(request.SectionId);

        existingSectionItem.Title = request.Title;
        existingSectionItem.Content = request.Content;
        existingSectionItem.SectionId = request.SectionId;
        existingSectionItem.UpdatedAt = DateTime.UtcNow;

        await _sectionItemRepository.Update(request.SectionItemId, existingSectionItem);

        if (oldSection != null)
        {
            oldSection.SectionItems.RemoveAll(s => s.SectionItemId == existingSectionItem.SectionItemId);
            await _sectionRepository.Update(oldSection.SectionId, oldSection);
        }

        if (newSection != null)
        {
            newSection.SectionItems.Add(existingSectionItem);
            await _sectionRepository.Update(newSection.SectionId, newSection);
        }

        return new SectionItemDto
        {
            SectionItemId = existingSectionItem.SectionItemId,
            Title = existingSectionItem.Title,
            Content = existingSectionItem.Content,
            SectionId = existingSectionItem.SectionId
        };
    }
}