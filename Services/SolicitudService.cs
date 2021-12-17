using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Domain.Services.Communication;
using kairosApp.Models;

namespace kairosApp.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SolicitudService(ISolicitudRepository solicitudRepository, IUnitOfWork unitOfWork)
        {
            _solicitudRepository = solicitudRepository;
            _unitOfWork = unitOfWork;
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

            existingSolicitud.Estado = solicitud.Estado;


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
