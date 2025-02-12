using Domain;

namespace Application.Common.Interfaces.Queries;

public interface IUserQueries
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(string id);
}