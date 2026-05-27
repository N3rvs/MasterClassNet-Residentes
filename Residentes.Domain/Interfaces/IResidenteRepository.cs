using Residentes.Domain.Entities;

namespace Residentes.Domain.Interfaces
{
    // ─────────────────────────────────────────────────────────────────────────────
    // INTERFAZ DE REPOSITORIO (vive en Domain)
    // ─────────────────────────────────────────────────────────────────────────────
    // ¿Por qué la interfaz vive en Domain y no en Infrastructure?
    //
    // Porque el Domain es quien DICE qué necesita ("yo necesito poder guardar y
    // recuperar residentes"), pero NO quiere saber CÓMO se hace (¿SQL Server?
    // ¿MongoDB? ¿una lista en memoria?). Esto se conoce como "Inversión de
    // Dependencias" (la D de SOLID).
    //
    // El proyecto Infrastructure es quien implementa esta interfaz. Así, si mañana
    // cambiamos de motor de base de datos, el Domain y el Application no se enteran.
    // ─────────────────────────────────────────────────────────────────────────────
    public interface IResidenteRepository
    {
        void Add(Residente residente);
        void Update(Residente residente);
        void Delete(Guid id);
        IEnumerable<Residente> GetAll();
        Residente? GetById(Guid id);
        Residente? GetByName(string nombre);
    }
}
