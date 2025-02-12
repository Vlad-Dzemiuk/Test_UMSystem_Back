using API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Commands;
using Domain;
using MediatR;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userQueries;
        private readonly IMediator _mediator;

        public UserController(IUserRepository userRepository, IUserQueries userQueries, IMediator? mediator)
        {
            _userRepository = userRepository;
            _userQueries = userQueries;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userQueries.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var users = await _userQueries.GetById(id);
                return Ok(users);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody] CreateUserDto createUserDto)
        {
            var user = new User
            {
                FirstName = createUserDto.FirstName,
                MiddleName = createUserDto.MiddleName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                ProfilePicture = createUserDto.ProfilePicture,
                Sections = new List<Section>(), // Порожній список секцій
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.Create(user);

            var userDto = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Sections = user.Sections
            };

            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, userDto);
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var existingUser = await _userQueries.GetById(id);
                if (existingUser == null)
                    return NotFound($"User with id {id} not found.");

                // Оновлюємо дані користувача
                existingUser = new User
                {
                    UserId = id,
                    FirstName = updateUserDto.FirstName,
                    MiddleName = updateUserDto.MiddleName,
                    LastName = updateUserDto.LastName,
                    Email = updateUserDto.Email,
                    ProfilePicture = updateUserDto.ProfilePicture,
                    Sections = existingUser.Sections, // Не змінюємо секції
                    CreatedAt = existingUser.CreatedAt, // Не змінюємо дату створення
                    UpdatedAt = DateTime.UtcNow // Оновлюємо дату редагування
                };

                await _userRepository.Update(id, existingUser);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var existingUser = await _userQueries.GetById(id);
                if (existingUser == null)
                    return NotFound($"User with id {id} not found.");

                await _userRepository.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}