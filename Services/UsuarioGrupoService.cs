using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Domain.Services.Communication;
using kairosApp.Models;

namespace kairosApp.Services
{
    public class UsuarioGrupoService : IUsuarioGrupoService
    {
        private readonly IUsuarioGrupoRepository _usuarioGrupoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioGrupoService(IUsuarioGrupoRepository usuarioGrupoRepository, IUnitOfWork unitOfWork)
        {
            _usuarioGrupoRepository = usuarioGrupoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UsuarioGrupo>> ListAsync()
        {
            return await _usuarioGrupoRepository.ListAsync();
        }

        public async Task<SaveUsuarioGrupoResponse> SaveAsync(UsuarioGrupo usuarioGrupo)
        {
            try
            {
                await _usuarioGrupoRepository.AddAsync(usuarioGrupo);
                await _unitOfWork.CompleteAsync();

                return new SaveUsuarioGrupoResponse(usuarioGrupo);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveUsuarioGrupoResponse($"Un error ocurrio mientras se guardaba el grupo usuario: {ex.Message}");
            }
        }

        public async Task<SaveUsuarioGrupoResponse> UpdateAsync(int id, UsuarioGrupo usuarioGrupo)
        {
            var existingUsuarioGrupo = await _usuarioGrupoRepository.FindByIdAsync(id);

            if (existingUsuarioGrupo == null)
                return new SaveUsuarioGrupoResponse("Usuario grupo no Encontrada.");

            existingUsuarioGrupo.GrupoId = usuarioGrupo.GrupoId;
            existingUsuarioGrupo.CuentaUsuarioId = usuarioGrupo.CuentaUsuarioId;

            try
            {
                _usuarioGrupoRepository.Update(existingUsuarioGrupo);
                await _unitOfWork.CompleteAsync();

                return new SaveUsuarioGrupoResponse(existingUsuarioGrupo);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveUsuarioGrupoResponse($"Un error ocurrio mientras se actualizaba a la persona: {ex.Message}");
            }
        }
    }
}
