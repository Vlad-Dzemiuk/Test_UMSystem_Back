using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken);

    Task<User> Create(User user, CancellationToken cancellationToken);
    Task<User> Update(User user, CancellationToken cancellationToken);
    Task<User> Delete(User user, CancellationToken cancellationToken);
}