namespace SociePolar.Application.Interfaces
{
    public interface IRepositoryFactory
    {
        // Retorna el repositorio como objeto para manejarlo dinámicamente
        object GetRepository(string entityName);
    }
}
