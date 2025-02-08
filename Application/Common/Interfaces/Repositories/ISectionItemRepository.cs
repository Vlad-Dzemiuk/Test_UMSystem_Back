using Domain.Section_Items;

namespace Application.Common.Interfaces.Repositories;

public interface ISectionItemRepository
{
    Task Create(SectionItem section);
    Task Update(string id, SectionItem section);
    Task Delete(string id);
}