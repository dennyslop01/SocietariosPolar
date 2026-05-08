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
    }
}