using Domain.Section_Items;

namespace Application.Common.Interfaces.Queries;

public interface ISectionItemQueries
{
    Task<IEnumerable<SectionItem>> GetAll();
    Task<SectionItem> GetById(string id);
}