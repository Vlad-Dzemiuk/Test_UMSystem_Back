using Domain;

namespace Application.Common.Interfaces.Repositories;

public interface ISectionItemRepository
{
    Task<SectionItem> GetById(string id);
    
    Task Create(SectionItem section);
    Task Update(string id, SectionItem section);
    Task Delete(string id);
}