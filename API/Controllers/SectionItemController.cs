using API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Commands;
using Application.SectionItems.Exceptions;
using Domain;
using MediatR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionItemController : ControllerBase
    {
        private readonly ISectionItemRepository _sectionItemRepository;
        private readonly ISectionItemQueries _sectionItemQueries;
        private readonly IMediator _mediator;
        
        public SectionItemController(ISectionItemRepository sectionItemRepository, ISectionItemQueries sectionItemQueries,
            IMediator? mediator)
        {
            _sectionItemRepository = sectionItemRepository;
            _sectionItemQueries = sectionItemQueries;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectionItem>>> GetAll()
        {
            var sectionItems = await _sectionItemQueries.GetAll();
            
            return Ok(sectionItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SectionItem>> GetById(string id)
        {
            try
            {
                var sectionItems = await _sectionItemQueries.GetById(id);
                return Ok(sectionItems);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<SectionItemDto>> Post([FromBody] CreateSectionItemDto dto)
        {
            try
            {
                var command = new CreateSectionItemsCommand(dto);
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.SectionItemId }, result);
            }
            catch (SectionItemAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<SectionItemDto>> Put(string id, [FromBody] UpdateSectionItemDto dto)
        {
            try
            {
                var command = new UpdateSectionItemsCommand(id, dto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
            
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var command = new DeleteSectionItemsCommand(id);
                await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}