using API.Dtos;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Exceptions;
using Application.Sections.Commands;
using Application.Sections.Exceptions;
using Domain;
using MediatR;

namespace Application.SectionItems.Commands;

public class CreateSectionItemsCommand : IRequest<SectionItemDto>
{
    public string SectionId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public CreateSectionItemsCommand(CreateSectionItemDto dto)
    {
        SectionId = dto.SectionId;
        Title = dto.Title;
        Content = dto.Content;
    }
}

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionItemsCommand, SectionItemDto>
{
    private readonly ISectionItemRepository _sectionItemRepository;
    private readonly ISectionItemQueries _sectionItemQueries;
    private readonly ISectionRepository _sectionRepository;
    private readonly ISectionQueries _sectionQueries;

    public CreateSectionCommandHandler(ISectionItemRepository sectionItemRepository, ISectionItemQueries sectionItemQueries, ISectionQueries sectionQueries, ISectionRepository sectionRepository)
    {
        _sectionItemRepository = sectionItemRepository;
        _sectionItemQueries = sectionItemQueries;
        _sectionQueries = sectionQueries;
        _sectionRepository = sectionRepository;
    }

    public async Task<SectionItemDto> Handle(CreateSectionItemsCommand request, CancellationToken cancellationToken)
    {
        var existingSectionItems = await _sectionItemQueries.GetAll();
        if (existingSectionItems.Any(u => u.Title == request.Title))
        {
            throw new SectionItemAlreadyExistsException(request.Title);
        }

        var sectionItem = new SectionItem
        {
            Title = request.Title,
            Content = request.Content,
            SectionId = request.SectionId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _sectionItemRepository.Create(sectionItem);

        var section = await _sectionQueries.GetById(request.SectionId);
        if (section != null)
        {
            section.SectionItems.Add(sectionItem);
            await _sectionRepository.Update(section.SectionId, section);
        }
        
        return new SectionItemDto
        {
            SectionItemId = sectionItem.SectionItemId,
            Title = sectionItem.Title,
            Content = sectionItem.Content,
            SectionId = sectionItem.SectionId
        };
    }
}