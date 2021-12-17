using kairosApp.Models;

namespace kairosApp.Domain.Repositories
{
    public interface ISolicitudRepository
    {
        Task<IEnumerable<Solicitud>> ListAsync();
        Task AddAsync(Solicitud solicitud);
        Task<Solicitud> FindByIdAsync(int id);
        void Update(Solicitud solicitud);
    }
}
