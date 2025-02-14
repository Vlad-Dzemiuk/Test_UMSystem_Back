using API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Commands;
using Application.Users.Commands;
using Domain;
using MediatR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly ISectionQueries _sectionQueries;
        private readonly IMediator _mediator;

        public SectionController(ISectionRepository sectionRepository, ISectionQueries sectionQueries,
            IMediator? mediator)
        {
            _sectionRepository = sectionRepository;
            _sectionQueries = sectionQueries;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> GetAll()
        {
            var sections = await _sectionQueries.GetAll();
            
            return Ok(sections);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Section>> GetById(string id)
        {
            try
            {
                var sections = await _sectionQueries.GetById(id);
                return Ok(sections);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<SectionDto>> Post([FromBody] CreateSectionDto dto)
        {
            var command = new CreateSectionCommand(dto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.SectionId }, result);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<SectionDto>> Put(string id, [FromBody] UpdateSectionDto dto)
        {
            var command = new UpdateSectionCommand(id, dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteSectionCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}