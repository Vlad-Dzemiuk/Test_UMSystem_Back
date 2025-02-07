using Domain.Section_Items;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ISection_ItemsQueries
{
    Task<IReadOnlyList<Section_Items>> GetAll(CancellationToken cancellationToken);
    Task<Option<Section_Items>> GetById(Section_ItemsId id, CancellationToken cancellationToken);
}