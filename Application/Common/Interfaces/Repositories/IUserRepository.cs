using Domain.Users;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetById(string id);
    
    Task Create(User user);
    Task Update(string id, User user);
    Task Delete(string id);
}