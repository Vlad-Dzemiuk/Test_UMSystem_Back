using Domain.Section_Items;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ISection_ItemsRepository
{
    Task<Option<Section_Items>> GetById(Section_ItemsId id, CancellationToken cancellationToken);

    Task<Section_Items> Create(Section_Items section_Items, CancellationToken cancellationToken);
    Task<Section_Items> Update(Section_Items section_Items, CancellationToken cancellationToken);
    Task<Section_Items> Delete(Section_Items section_Items, CancellationToken cancellationToken);
}