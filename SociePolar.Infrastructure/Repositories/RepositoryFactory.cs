using Microsoft.Extensions.DependencyInjection;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Entities;

namespace SociePolar.Infrastructure.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;
        // Mapeo de nombres a tipos reales
        private readonly Dictionary<string, Type> _entityTypes;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            // Aquí registras las entidades que quieres permitir
            _entityTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "cargos", typeof(Cargo) },
                { "claselibros", typeof(ClaseLibro) },
                { "empresas", typeof(Empresa) },
                { "estatussociedad", typeof(EstatusSociedad) },
                { "nombrediarios", typeof(NombreDiario) },
                { "regiones", typeof(Region) },
                { "registros", typeof(Registro) },
                { "tipoasambleas", typeof(TipoAsamblea) },
                { "tiporeformas", typeof(TipoReforma) },
                { "tiposociedad", typeof(TipoSociedad) },
                { "tiposociedadactiva", typeof(TipoSociedadActiva) },
            };
        }

        public object GetRepository(string entityName)
        {
            if (!_entityTypes.TryGetValue(entityName, out var type))
                throw new Exception($"Entidad {entityName} no soportada");

            // Crea el tipo genérico: IGenericRepository<TipoSeleccionado>
            var repositoryType = typeof(IGenericRepository<>).MakeGenericType(type);
            return _serviceProvider.GetRequiredService(repositoryType);
        }
    }
}