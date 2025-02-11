using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Sections;
using Domain.Users;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly ISectionQueries _sectionQueries;


        public SectionController(ISectionRepository sectionRepository, ISectionQueries sectionQueries)
        {
            _sectionRepository = sectionRepository;
            _sectionQueries = sectionQueries;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> Get()
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
        public async Task<ActionResult<Section>> Post(Section section)
        {
            await _sectionRepository.Create(section);
            return CreatedAtAction(nameof(GetById), new { id = section.Id }, section);
        }
    }    
}
