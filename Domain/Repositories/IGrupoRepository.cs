using kairosApp.Models;

namespace kairosApp.Domain.Repositories
{
    public interface IGrupoRepository
    {
        Task<IEnumerable<Grupo>> ListAsync();
        Task AddAsync(Grupo grupo);
        Task<Grupo> FindByIdAsync(int id);
        void Update(Grupo grupo);
    }
}
