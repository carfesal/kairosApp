using AutoMapper;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using kairosApp.Domain.Services;

namespace kairosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaUsuarioController : ControllerBase
    {
        private readonly ICuentaUsuarioService _cuentaUsuarioService;
        private readonly IMapper _mapper;
        public CuentaUsuarioController(ICuentaUsuarioService cuentaUsuarioService, IMapper mapper)
        {
            _cuentaUsuarioService = cuentaUsuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CuentaUsuarioResource>> GetAllAsync()
        {
            var accounts = await _cuentaUsuarioService.ListAsync();
            var rsc = _mapper.Map<IEnumerable<CuentaUsuario>, IEnumerable<CuentaUsuarioResource>>(accounts);

            return rsc;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveCuentaUsuarioResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var cuenta = _mapper.Map<SaveCuentaUsuarioResource, CuentaUsuario>(resource);

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
    }
}
