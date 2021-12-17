using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PersonaService(IPersonaRepository personaRepository, IUnitOfWork unitOfWork)
        {
            _personaRepository = personaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Persona>> ListAsync()
        {
            return await _personaRepository.ListAsync();
        }

        public async Task<SavePersonaResponse> SaveAsync(Persona persona)
        {
            try
            {
                await _personaRepository.AddAsync(persona);
                await _unitOfWork.CompleteAsync();

                return new SavePersonaResponse(persona);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SavePersonaResponse($"Un error ocurrio mientras se guardaba la persona: {ex.Message}");
            }
        }

        public async Task<SavePersonaResponse> UpdateAsync(int id, Persona persona)
        {
            var existingPersona = await _personaRepository.FindByIdAsync(id);

            if (existingPersona == null)
                return new SavePersonaResponse("Persona no Encontrada.");

            existingPersona.Nombres = persona.Nombres;
            existingPersona.Apellidos = persona.Apellidos;
            existingPersona.Telefono = persona.Telefono;
            existingPersona.Identificacion = persona.Identificacion;
            existingPersona.Rol = persona.Rol;

            try
            {
                _personaRepository.Update(existingPersona);
                await _unitOfWork.CompleteAsync();

                return new SavePersonaResponse(existingPersona);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SavePersonaResponse($"Un error ocurrio mientras se actualizaba a la persona: {ex.Message}");
            }
        }
    }
}
