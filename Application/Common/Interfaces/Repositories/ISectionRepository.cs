using Domain.Sections;

namespace Application.Common.Interfaces.Repositories;

public interface ISectionRepository
{
    Task Create(Section section);
    Task Update(string id, Section section);
    Task Delete(string id);
}