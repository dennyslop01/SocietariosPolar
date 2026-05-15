using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class UsuarioRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IUsuario
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            // Creamos una instancia local para esta operación específica
            using var context = _contextFactory.CreateDbContext();

            try
            {
                Usuario? usuario = await context.Usuarios
                    .Where(x => x.Email == email)
                    .FirstOrDefaultAsync();
                return usuario;
            }
            catch (Exception ex)
            {
                // Es recomendable usar un Logger en lugar de Console.WriteLine
                Console.WriteLine($"Error al obtener el empleado por email: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Usuario>()
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Usuario>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task AddAsync(Usuario entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<Usuario>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public void Update(Usuario entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<Usuario>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<Usuario>().Find(id);
            if (entity != null)
            {
                context.Set<Usuario>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}