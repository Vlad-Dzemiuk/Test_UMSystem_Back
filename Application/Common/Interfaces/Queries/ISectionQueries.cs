using Domain.Sections;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ISectionQueries
{
    Task<IReadOnlyList<Section>> GetAll(CancellationToken cancellationToken);
    Task<Option<Section>> GetById(SectionId id, CancellationToken cancellationToken);
}