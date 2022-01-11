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
        private readonly IActiveDirectoryService _activeDirectoryService;
        public PersonaController(IPersonaService personaService, IMapper mapper, IActiveDirectoryService activeDirectoryService)
        {
            _personaService = personaService;
            _mapper = mapper;
            _activeDirectoryService = activeDirectoryService;
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

        [HttpGet]
        [Route("{identificacion}/{fecha}/{tipo}")]
        public async Task<IActionResult> GetPerson(string identificacion, string fecha, string tipo) 
        {
            //ACCION A LA OTRA BASE DE DATOS 
            var persona = _personaService.GetPersonaByCedula(identificacion);
            if (persona == null) { return NotFound(new ErrorResource { ErrorMessage = "No se encontro persona con esa cedula"}); }
            return Ok(persona);
        }

        [HttpGet]
        [Route("cuenta/{identificacion}/{fecha}/{tipo}")]
        public async Task<IActionResult> GetPersonWithAccount(string identificacion, string fecha, string tipo)
        {
            //ACCION A LA OTRA BASE DE DATOS p ACTIVE DIRECTORY
            return Ok("Se debe retornar info de la persona.");
        }
    }

    
}
