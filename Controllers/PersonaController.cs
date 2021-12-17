using AutoMapper;
using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaService _personaService;
        private readonly IMapper _mapper;
        public PersonaController(IPersonaService personaService, IMapper mapper)
        {
            _personaService = personaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonaResource>> GetAllAsync()
        {
            var people = await _personaService.ListAsync();
            var rsc = _mapper.Map<IEnumerable<Persona>, IEnumerable<PersonaResource>>(people);

            return rsc;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SavePersonaResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            
            var persona = _mapper.Map<SavePersonaResource, Persona>(resource);

            var result = await _personaService.SaveAsync(persona);

            if (!result.Success)
                return BadRequest(result.Message);

            var personaResource = _mapper.Map<Persona, PersonaResource>(result.Persona);
            return Ok(personaResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SavePersonaResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var persona = _mapper.Map<SavePersonaResource, Persona>(resource);
            var result = await _personaService.UpdateAsync(id, persona);

            if (!result.Success)
                return BadRequest(result.Message);

            var personaResource = _mapper.Map<Persona, PersonaResource>(result.Persona);
            return Ok(personaResource);
        }
    }
}
