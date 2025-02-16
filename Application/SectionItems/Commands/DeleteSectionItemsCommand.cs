using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Exceptions;
using MediatR;

namespace Application.SectionItems.Commands;

public class DeleteSectionItemsCommand : IRequest<Unit>
{
    public string SectionItemId { get; }

    public DeleteSectionItemsCommand(string sectionItemId)
    {
        SectionItemId = sectionItemId;
    }
}

public class DeleteSectionItemsCommandHandler : IRequestHandler<DeleteSectionItemsCommand, Unit>
{
    private readonly ISectionItemRepository _sectionItemRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly ISectionQueries _sectionQueries;

    public DeleteSectionItemsCommandHandler(ISectionItemRepository sectionItemRepository, ISectionRepository sectionRepository, ISectionQueries sectionQueries)
    {
        _sectionQueries = sectionQueries;
        _sectionItemRepository = sectionItemRepository;
        _sectionRepository = sectionRepository;
    }

    public async Task<Unit> Handle(DeleteSectionItemsCommand request, CancellationToken cancellationToken)
    {
        var existingSectionItem = await _sectionItemRepository.GetById(request.SectionItemId);
        if (existingSectionItem == null)
        {
            throw new SectionItemNotFoundException(request.SectionItemId);
        }

        var section = await _sectionQueries.GetById(existingSectionItem.SectionId);
        if (section != null)
        {
            section.SectionItems.RemoveAll(s => s.SectionItemId == existingSectionItem.SectionItemId);
            await _sectionRepository.Update(section.SectionId, section);
        }
        
        await _sectionItemRepository.Delete(request.SectionItemId);
        return Unit.Value;
    }
}