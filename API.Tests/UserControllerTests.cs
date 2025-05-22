using API.Controllers;
using API.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Commands;
using Application.Users.Exceptions;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace API.Tests;

public class UserControllerTests : BaseIntegrationTest
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IUserQueries> _userQueries = new();
    private readonly Mock<IMediator> _mediator = new();

    private readonly UserController _userController;

    public UserControllerTests()
    {
        _userController = new UserController(
            _userRepository.Object,
            _userQueries.Object,
            _mediator.Object
        );
    }
    
    [Fact]
    public async Task GetAll_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new() { FirstName = "Alice", UserId = "1" },
            new() { FirstName = "Bob", UserId = "2" }
        };

        _userQueries.Setup(q => q.GetAll()).ReturnsAsync(users);

        var result = await _userController.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count());
    }

    [Fact]
    public async Task GetById_ReturnsUser_WhenFound()
    {
        var id = ObjectId.GenerateNewId().ToString();
        var user = new User { UserId = id, FirstName = "Test" };

        _userQueries.Setup(q => q.GetById(id)).ReturnsAsync(user);

        var result = await _userController.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(id, returnedUser.UserId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenUserNotExists()
    {
        var id = ObjectId.GenerateNewId().ToString();

        _userQueries.Setup(q => q.GetById(id))
            .ThrowsAsync(new KeyNotFoundException("User not found"));

        var result = await _userController.GetById(id);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("User not found", notFound.Value);
    }

    [Fact]
    public async Task Post_CreatesUser_ReturnsCreatedUser()
    {
        var dto = new CreateUserDto
        {
            FirstName = "Alice",
            MiddleName = "L",
            LastName = "Smith",
            Email = "alice@example.com",
            ProfilePicture = "img.jpg"
        };

        var expected = new UserDto
        {
            UserId = ObjectId.GenerateNewId().ToString(),
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Email = dto.Email,
            ProfilePicture = dto.ProfilePicture
        };

        _mediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), default))
            .ReturnsAsync(expected);

        var result = await _userController.Post(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returned = Assert.IsType<UserDto>(createdResult.Value);
        Assert.Equal(dto.FirstName, returned.FirstName);
        Assert.Equal(dto.Email, returned.Email);
    }

    [Fact]
    public async Task Post_WhenUserAlreadyExists_ReturnsBadRequest()
    {
        var dto = new CreateUserDto
        {
            FirstName = "Alice",
            MiddleName = "L",
            LastName = "Smith",
            Email = "alice@example.com",
            ProfilePicture = "img.jpg"
        };

        _mediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), default))
            .ThrowsAsync(new UserAlreadyExistsException(dto.FirstName));

        var result = await _userController.Post(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        badRequest.Value.Should().Be($"Користувач з ім'ям \"{dto.FirstName}\" вже існує.");
    }

    [Fact]
    public async Task Put_UpdatesUser_ReturnsUpdatedUser()
    {
        var id = ObjectId.GenerateNewId().ToString();
        var dto = new UpdateUserDto
        {
            FirstName = "Updated",
            MiddleName = "M",
            LastName = "Lastname",
            Email = "updated@example.com",
            ProfilePicture = "new.jpg"
        };

        var expected = new UserDto
        {
            UserId = id,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Email = dto.Email,
            ProfilePicture = dto.ProfilePicture
        };

        _mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
            .ReturnsAsync(expected);

        var result = await _userController.Put(id, dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(expected.Email, returnedDto.Email);
    }

    [Fact]
    public async Task Put_WhenUserNotFound_ReturnsNotFound()
    {
        var id = ObjectId.GenerateNewId().ToString();
        var dto = new UpdateUserDto
        {
            FirstName = "Missing",
            MiddleName = "M",
            LastName = "Lastname",
            Email = "notfound@example.com",
            ProfilePicture = "none.jpg"
        };

        _mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
            .ThrowsAsync(new KeyNotFoundException("Користувача не знайдено"));

        var result = await _userController.Put(id, dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Користувача не знайдено", notFound.Value);
    }

    [Fact]
    public async Task Delete_RemovesUser_ReturnsOk()
    {
        var id = ObjectId.GenerateNewId().ToString();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
            .ReturnsAsync(Unit.Value);

        var result = await _userController.Delete(id);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_WhenUserNotFound_ReturnsNotFound()
    {
        var id = ObjectId.GenerateNewId().ToString();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
            .ThrowsAsync(new KeyNotFoundException("Користувача не знайдено"));

        var result = await _userController.Delete(id);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        notFound.Value.Should().Be("Користувача не знайдено");
    }
}