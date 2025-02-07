using Domain.Sections;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ISectionRepository
{
    Task<Option<Section>> GetById(SectionId id, CancellationToken cancellationToken);

    Task<Section> Create(Section section, CancellationToken cancellationToken);
    Task<Section> Update(Section section, CancellationToken cancellationToken);
    Task<Section> Delete(Section section, CancellationToken cancellationToken);
}