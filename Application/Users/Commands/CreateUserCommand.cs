using API.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain;
using MediatR;

namespace Application.Users.Commands;

public class CreateUserCommand : IRequest<UserDto>
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? ProfilePicture { get; set; }

    public CreateUserCommand(CreateUserDto dto)
    {
        FirstName = dto.FirstName;
        MiddleName = dto.MiddleName;
        LastName = dto.LastName;
        Email = dto.Email;
        ProfilePicture = dto.ProfilePicture;
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserQueries _userQueries;

    public CreateUserCommandHandler(IUserRepository userRepository, IUserQueries userQueries)
    {
        _userRepository = userRepository;
        _userQueries = userQueries;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUsers = await _userQueries.GetAll();
        if (existingUsers.Any(u => u.Email == request.Email))
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        var user = new User
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Email = request.Email,
            ProfilePicture = request.ProfilePicture,
            Sections = new List<Section>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.Create(user);

        return new UserDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Sections = user.Sections
        };
    }
}