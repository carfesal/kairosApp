using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Models;
using Microsoft.EntityFrameworkCore;

namespace kairosApp.Domain.Persistence.Repositories
{
    public class PersonaRepository : BaseRepository, IPersonaRepository
    {
        public PersonaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Persona persona)
        {
            await _context.Personas.AddAsync(persona);
        }

        public async Task<IEnumerable<Persona>> ListAsync()
        {
            return await _context.Personas.ToListAsync();
        }

        public async Task<Persona> FindByIdAsync(int id)
        {
            return await _context.Personas.FindAsync(id);
        }

        public void Update(Persona persona)
        {
            _context.Personas.Update(persona);
        }
    }
}
