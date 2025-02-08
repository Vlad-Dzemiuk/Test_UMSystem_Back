using Domain.Sections;

namespace Application.Common.Interfaces.Queries;

public interface ISectionQueries
{
    Task<IEnumerable<Section>> GetAll();
    Task<Section> GetById(string id);
}