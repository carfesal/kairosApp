using kairosApp.Domain.Services.Communication;
using kairosApp.Models;

namespace kairosApp.Domain.Services
{
    public interface ISolicitudService
    {
        Task<IEnumerable<Solicitud>> ListAsync();
        Task<SaveSolicitudResponse> SaveAsync(Solicitud solicitud);
        Task<SaveSolicitudResponse> UpdateAsync(int id, Solicitud solicitud);
    }
}
