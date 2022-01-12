using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Models;
using kairosApp.Domain.Services.Communication;
using kairosApp.Domain.Persistence.Contexts;

namespace kairosApp.Services
{
    public class CuentaUsuarioService : ICuentaUsuarioService
    {
        private readonly ICuentaUsuarioRepository _cuentaUsuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public CuentaUsuarioService(ICuentaUsuarioRepository cuentaUsuarioRepository, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _cuentaUsuarioRepository = cuentaUsuarioRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<CuentaUsuario>> ListAsync()
        {
            return await _cuentaUsuarioRepository.ListAsync();
        }

        public async Task<SaveCuentaUsuarioResponse> SaveAsync(CuentaUsuario cuenta)
        {
            try
            {
                await _cuentaUsuarioRepository.AddAsync(cuenta);
                await _unitOfWork.CompleteAsync();

                return new SaveCuentaUsuarioResponse(cuenta);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveCuentaUsuarioResponse($"Un error ocurrio mientras se guardaba la cuenta: {ex.Message}");
            }
        }

        public async Task<SaveCuentaUsuarioResponse> UpdateAsync(int id, CuentaUsuario cuentaUsuario)
        {
            var existingCuenta = await _cuentaUsuarioRepository.FindByIdAsync(id);

            if (existingCuenta == null)
                return new SaveCuentaUsuarioResponse("Cuenta no Encontrada.");

            /*existingCuenta.Alias = cuentaUsuario.Alias;
            existingCuenta.Username = cuentaUsuario.Username;*/
            existingCuenta.IsActive = cuentaUsuario.IsActive;

            try
            {
                _cuentaUsuarioRepository.Update(existingCuenta);
                await _unitOfWork.CompleteAsync();

                return new SaveCuentaUsuarioResponse(existingCuenta);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveCuentaUsuarioResponse($"Un error ocurrio mientras se actualizaba a la persona: {ex.Message}");
            }
        }

        public bool VerifyAlias(string alias)
        {
            var aliasExistente = _context.CuentaUsuarios.Where(p => p.Alias == alias).Any();
            return aliasExistente;
        }
    }
}
