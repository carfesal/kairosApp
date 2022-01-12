using kairosApp.Domain.Services.Communication;
using kairosApp.Models;

namespace kairosApp.Domain.Services
{
    public interface ICuentaUsuarioService
    {
        Task<IEnumerable<CuentaUsuario>> ListAsync();
        Task<SaveCuentaUsuarioResponse> SaveAsync(CuentaUsuario cuentaUsuario);
        Task<SaveCuentaUsuarioResponse> UpdateAsync(int id, CuentaUsuario cuentaUsuario);
        bool VerifyAlias(string alias);
    }
}
