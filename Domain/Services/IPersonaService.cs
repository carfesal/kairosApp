using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Services
{
    public interface IPersonaService
    {
        Task<IEnumerable<Persona>> ListAsync();
        Task<SavePersonaResponse> SaveAsync(Persona persona);
        Task<SavePersonaResponse> UpdateAsync(int id, Persona persona);
    }
}
