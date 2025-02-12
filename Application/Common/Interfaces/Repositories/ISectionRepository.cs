using Domain;

namespace Application.Common.Interfaces.Repositories;

public interface ISectionRepository
{
    Task<Section> GetById(string id);
    
    Task Create(Section section);
    Task Update(string id, Section section);
    Task Delete(string id);
}