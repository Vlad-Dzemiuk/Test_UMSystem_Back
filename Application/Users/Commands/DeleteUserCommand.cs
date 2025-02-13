using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using MediatR;

namespace Application.Users.Commands;

public class DeleteUserCommand : IRequest<Unit>
{
    public string UserId { get; }

    public DeleteUserCommand(string userId)
    {
        UserId = userId;
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetById(request.UserId);
        if (existingUser == null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        await _userRepository.Delete(request.UserId);
        return Unit.Value;
    }
}