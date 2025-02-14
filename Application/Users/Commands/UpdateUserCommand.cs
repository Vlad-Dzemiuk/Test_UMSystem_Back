using API.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserCommand : IRequest<UserDto>
{
    public string UserId { get; }
    public string FirstName { get; }
    public string MiddleName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string? ProfilePicture { get; }

    public UpdateUserCommand(string userId, UpdateUserDto dto)
    {
        UserId = userId;
        FirstName = dto.FirstName;
        MiddleName = dto.MiddleName;
        LastName = dto.LastName;
        Email = dto.Email;
        ProfilePicture = dto.ProfilePicture;
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserQueries _userQueries;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUserQueries userQueries)
    {
        _userRepository = userRepository;
        _userQueries = userQueries;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userQueries.GetById(request.UserId) 
                           ?? throw new UserNotFoundException(request.UserId);

        existingUser.FirstName = request.FirstName;
        existingUser.MiddleName = request.MiddleName;
        existingUser.LastName = request.LastName;
        existingUser.Email = request.Email;
        existingUser.ProfilePicture = request.ProfilePicture;
        existingUser.UpdatedAt = DateTime.UtcNow;

        await _userRepository.Update(request.UserId, existingUser);

        return new UserDto
        {
            UserId = existingUser.UserId,
            FirstName = existingUser.FirstName,
            MiddleName = existingUser.MiddleName,
            LastName = existingUser.LastName,
            Email = existingUser.Email,
            ProfilePicture = existingUser.ProfilePicture,
            Sections = existingUser.Sections
        };
    }
}
