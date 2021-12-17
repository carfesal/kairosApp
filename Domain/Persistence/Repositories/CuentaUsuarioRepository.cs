using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Models;
using Microsoft.EntityFrameworkCore;

namespace kairosApp.Domain.Persistence.Repositories
{
    public class CuentaUsuarioRepository : BaseRepository, ICuentaUsuarioRepository
    {
        public CuentaUsuarioRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(CuentaUsuario cuentaUsuario)
        {
            await _context.CuentaUsuarios.AddAsync(cuentaUsuario);
        }

        public async Task<IEnumerable<CuentaUsuario>> ListAsync()
        {
            return await _context.CuentaUsuarios.ToListAsync();
        }

        public async Task<CuentaUsuario> FindByIdAsync(int id)
        {
            return await _context.CuentaUsuarios.FindAsync(id);
        }

        public void Update(CuentaUsuario cuentaUsuario)
        {
            _context.CuentaUsuarios.Update(cuentaUsuario);
        }
    }
}
