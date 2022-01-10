using kairosApp.Models;

namespace kairosApp.Domain.Repositories
{
    public interface IUsuarioGrupoRepository
    {
        Task<IEnumerable<UsuarioGrupo>> ListAsync();
        Task AddAsync(UsuarioGrupo usuarioGrupo);
        Task<UsuarioGrupo> FindByIdAsync(int id);
        void Update(UsuarioGrupo usuarioGrupo);
    }
}
