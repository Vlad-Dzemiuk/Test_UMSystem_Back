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
        public async Task<ActionResult<UserDto>> Post([FromBody] CreateUserDto dto)
        {
            var command = new CreateUserCommand(dto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.UserId }, result);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Put(string id, [FromBody] UpdateUserDto dto)
        {
            var command = new UpdateUserCommand(id, dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}