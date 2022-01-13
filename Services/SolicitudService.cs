using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using Newtonsoft.Json;

namespace kairosApp.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public SolicitudService(ISolicitudRepository solicitudRepository, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _solicitudRepository = solicitudRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<Solicitud>> ListAsync()
        {
            return await _solicitudRepository.ListAsync();
        }

        public async Task<SaveSolicitudResponse> SaveAsync(Solicitud solicitud)
        {
            try
            {
                await _solicitudRepository.AddAsync(solicitud);
                await _unitOfWork.CompleteAsync();

                return new SaveSolicitudResponse(solicitud);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveSolicitudResponse($"Un error ocurrio mientras se guardaba solicitud: {ex.Message}");
            }
        }

        public async Task<SaveSolicitudResponse> UpdateAsync(int id, Solicitud solicitud)
        {
            var existingSolicitud = await _solicitudRepository.FindByIdAsync(id);

            if (existingSolicitud == null)
                return new SaveSolicitudResponse("solicitud no Encontrada.");

            
            if(solicitud.Estado == "Aceptado")
            {
                existingSolicitud.Estado = solicitud.Estado;
                var info = JsonConvert.DeserializeObject<InfoSolicitud>(existingSolicitud.InfoSolicitud);
                var username = info.usuario_sugerido;
                var alias = info.alias_sugerido;

                var usernameBuscado = _context.CuentaUsuarios.Where(p => p.Username == username).ToList();
                var aliasBuscado = _context.CuentaUsuarios.Where(p =>p.Alias == alias).ToList();

                if(usernameBuscado.Any() || aliasBuscado.Any())
                {
                    return new SaveSolicitudResponse("Nombres de usuario o alias ya existentes.");
                }

                Persona persona = new Persona() { Nombres = info.nombres, Apellidos = info.apellidos, CorreoAlterno = info.correo, Identificacion = info.identificacion, Rol = info.actividad, Unidad = info.unidad, Telefono = info.telefono};
                _context.Personas.Add(persona);
                _context.SaveChanges();
                CuentaUsuario cu = new CuentaUsuario() { Username = info.usuario_sugerido, Alias = info.alias_sugerido, PersonaId = persona.Id, IsActive=true};
                _context.CuentaUsuarios.Add(cu);
                _context.SaveChanges();

            }else if (solicitud.Estado == "Rechazado")
            {
                existingSolicitud.Estado = solicitud.Estado;
            }
            else
            {
                return new SaveSolicitudResponse("Estado Incorrecto: Solo se acepta estado 'Rechazado' o 'Aceptado'");
            }

            try
            {
                _solicitudRepository.Update(existingSolicitud);
                await _unitOfWork.CompleteAsync();

                return new SaveSolicitudResponse(existingSolicitud);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveSolicitudResponse($"Un error ocurrio mientras se actualizaba Solicitud: {ex.Message}");
            }
        }
    }
}
