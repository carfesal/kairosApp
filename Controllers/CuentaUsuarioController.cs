using AutoMapper;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Resources;

using Microsoft.AspNetCore.Mvc;
using kairosApp.Domain.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;
using kairosApp.Helpers;
using System.Diagnostics;
using kairosApp.Resources.Support;
using kairosApp.Models.Support;
using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Models.Support.Mail;

namespace kairosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaUsuarioController : ControllerBase
    {
        private readonly ICuentaUsuarioService _cuentaUsuarioService;
        private readonly IMapper _mapper;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;

        private IEnumerable<CuentaUsuario> logins = new List<CuentaUsuario>() {
            new CuentaUsuario() {
                    Id = 1,
                        Alias = "carlos.salazar",
                        Username = "carfesal",
                },
                new CuentaUsuario() {
                    Id = 2,
                        Alias = "melanie.banchon",
                        Username = "meldaban",
                }
        };
        public CuentaUsuarioController(ICuentaUsuarioService cuentaUsuarioService, IMapper mapper, IActiveDirectoryService activeDirectoryService, JwtSettings jwtSettings, AppDbContext context, IEmailSender emailSender)
        {
            _cuentaUsuarioService = cuentaUsuarioService;
            _mapper = mapper;
            _activeDirectoryService = activeDirectoryService;
            _jwtSettings = jwtSettings;
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IEnumerable<CuentaUsuarioResource>> GetAllAsync()
        {
            var accounts = await _cuentaUsuarioService.ListAsync();
            foreach (var account in accounts)
            {
                Debug.WriteLine(account.Username+" "+account.PersonaId);
            }
            var rsc = _mapper.Map<IEnumerable<CuentaUsuario>, IEnumerable<CuentaUsuarioResource>>(accounts);
            var retornable = rsc.Where(p => p.IsActive == true);
            return retornable;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CrearCuentaResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            var cuentaRsc = new SaveCuentaUsuarioResource { Alias = resource.Alias, IsActive = true, Username = resource.Username, PersonaId=resource.PersonaId};
            var cuenta = _mapper.Map<SaveCuentaUsuarioResource, CuentaUsuario>(cuentaRsc);

            var result = await _cuentaUsuarioService.SaveAsync(cuenta);

            if (!result.Success)
                return BadRequest(result.Message);

            var cuentaResource = _mapper.Map<CuentaUsuario, CuentaUsuarioResource>(result.CuentaUsuario);
            return Ok(cuentaResource);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCuentaUsuarioResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var cuenta = _mapper.Map<SaveCuentaUsuarioResource, CuentaUsuario>(resource);
            var result = await _cuentaUsuarioService.UpdateAsync(id, cuenta);

            if (!result.Success)
                return BadRequest(result.Message);

            var cuentaUsuarioResource = _mapper.Map<CuentaUsuario, CuentaUsuarioResource>(result.CuentaUsuario);
            return Ok(cuentaUsuarioResource);
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserCredentials credentials)
        {
            /*try
            {
                var usuario = _activeDirectoryService.Login(credentials.Username, credentials.Password);
                if (usuario == null)
                {
                    return BadRequest("Credenciales erroneas");
                }
                return Ok(usuario);
                    
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }*/
            try
            {
                var Token = new UserTokens();
                var Valid = _context.CuentaUsuarios.Any(x => x.Username == credentials.Username);
                if (Valid)
                {
                    var user = _context.CuentaUsuarios.FirstOrDefault(x => x.Username == credentials.Username);
                    if (user == null) { return NotFound(new ErrorResource { ErrorMessage = "Usuario no encontrado" }); }

                    var persona = _context.Personas.FirstOrDefault(x => x.Id == user.PersonaId);
                    Debug.WriteLine(persona.Nombres);
                    Token = JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        Username = user.Username,
                        Id = user.Id,
                        Persona = persona,
                    }, _jwtSettings);
                }
                else
                {
                    return BadRequest(new ErrorResource { ErrorMessage = "Usuario y/o contraseña erroneos"});
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        [Route("change")]
        public async Task<IActionResult> ChangePassword([FromBody] UserCredentials credentials)
        {
            bool info = false;
            //CODIGO DEL ACTIVE DIRECTORY SERVICE
            _activeDirectoryService.GetAdditionalUserInfo();
            //var respuesta = _activeDirectoryService.CreateUser(new ADCreateUser { Persona = null, Username = "csrlod"});
            //Debug.WriteLine("Se creo el usuario: "+ respuesta);
            return Ok("Contraseña cambiada exitosamente");
        }

        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] UserCredentials credentials)
        {
            bool info = false;
            //CODIGO DEL ACTIVE DIRECTORY SERVICE

            if (info)
            {
                return NotFound("Credenciales Erroneas.");
            }
            return Ok("Contraseña cambiada exitosamente");
        }
        
        [HttpPost]
        [Route("verificaremail")]
        public async Task<IActionResult> verifyEmail ( [FromBody] PersonCredentials credentials)
        {
            var respuesta = _cuentaUsuarioService.VerifyEmail(credentials);
            if (!respuesta)
            {
                return BadRequest(new ErrorResource { ErrorMessage = "Correo alterno incorrecto."});
            }
            try
            {
                var newPassWord = _cuentaUsuarioService.CreateNewPassword();
                var message = new Message(new string[] { credentials.Correo }, "Cambio de contraseña", "La nueva contraseña para su cuenta es: " + newPassWord);
                _emailSender.SendEmail(message);
                return Ok(new ResponseResource { Success = true, Message = "Contraseña reseteada con exito."});
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResource { ErrorMessage = "Ha ocurrido un problema: " +ex.Message });
            }
            

        }
        [HttpGet]
        [Route("verificaralias/{alias}")]
        public async Task<IActionResult> verifyAlias(string alias)
        {
            var respuesta = _cuentaUsuarioService.VerifyAlias(alias);
            if (respuesta)
            {
                return Ok(new ResponseResource { Success = respuesta, Message = "Alias ya existente"});
            }
            return Ok(new ResponseResource { Success = respuesta, Message = "Alias no existe en base" });
        }

        [HttpGet]
        [Route("verificarusername/{username}")]
        public async Task<IActionResult> verifyUsername(string username)
        {
            var respuesta = _cuentaUsuarioService.VerifyUsername(username);
            if (respuesta)
            {
                return Ok(new ResponseResource { Success = respuesta, Message = "Username ya existente" });
            }
            return Ok(new ResponseResource { Success = respuesta, Message = "Username no existe en base" });
        }
    }
    

}
