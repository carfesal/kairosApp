using AutoMapper;
using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Models.Support;
using kairosApp.Resources;
using kairosApp.Resources.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly AppDbContext _context; 
        public PersonaController(IPersonaService personaService, IMapper mapper, IActiveDirectoryService activeDirectoryService, AppDbContext context)
        {
            _personaService = personaService;
            _mapper = mapper;
            _activeDirectoryService = activeDirectoryService;
            _context = context;
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
            if (persona == null) { return NotFound(new ErrorResource { ErrorMessage = "No se encontro persona con esa cedula."}); }
            if( persona.Persona == null) { return Ok(new ResponsePersona { Success = false, Persona = null}); }
            return Ok(new ResponsePersona { Success = true, Persona = persona });
        }

        [HttpGet]
        [Route("cuenta/{identificacion:minlength(10)}")]
        public async Task<IActionResult> GetPersonWithAccount(string identificacion)
        {
            //ACCION A LA OTRA BASE DE DATOS o ACTIVE DIRECTORY
            var persona = _personaService.GetPersonWithAccountByCedula(identificacion);
            if (persona == null) 
            {
                ADToDBUser adUser = _activeDirectoryService.FindUserByIdentification(identificacion);
                if(adUser == null) { return NotFound(new ErrorResource { ErrorMessage = "No se encontro persona con esa cedula." }); }
                if(adUser.Persona == null) { return NotFound(new ErrorResource { ErrorMessage = "No se encontro persona con esa cedula." }); }
                
                Debug.WriteLine("Entra al proceso de AD");
                //Se guarda la persona del active directory en la base
                Persona person = adUser.Persona;
                _context.Personas.Add(person);
                _context.SaveChanges();

                //Se guarda la CuentaUsuario en la base
                CuentaUsuario cu = adUser.CuentaUsuario;
                cu.IsActive = true;
                cu.PersonaId = person.Id;
                _context.CuentaUsuarios.Add(cu);
                _context.SaveChanges();
                return Ok(new PersonaConCuentaResource { Success = true , Persona = person});
            }
            return Ok(persona);
            
        }
    }

    
}
