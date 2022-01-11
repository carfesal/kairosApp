using kairosApp.Domain.Services.Communication;
using kairosApp.Models;

namespace kairosApp.Domain.Services
{
    public interface IUsuarioGrupoService
    {
        Task<IEnumerable<UsuarioGrupo>> ListAsync();
        Task<SaveUsuarioGrupoResponse> SaveAsync(UsuarioGrupo usuarioGrupo);
        Task<SaveUsuarioGrupoResponse> UpdateAsync(int id, UsuarioGrupo usuarioGrupo);

        bool UpdateUsuarioGrupos(int usuarioId, List<int> gruposIds);
    }
}
