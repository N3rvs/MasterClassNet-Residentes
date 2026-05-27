using Residentes.Domain.Entities;
using Residentes.Domain.Interfaces;

namespace Residentes.Infraestructure.Repositories
{
    // ─────────────────────────────────────────────────────────────────────────────
    // IMPLEMENTACIÓN DEL REPOSITORIO (vive en Infrastructure)
    // ─────────────────────────────────────────────────────────────────────────────
    // Aquí es donde decimos CÓMO se persiste la información. En este ejemplo
    // didáctico usamos una lista en memoria (static) para no complicarnos con
    // bases de datos. Más adelante, sustituirás esto por EF Core + SQL Server
    // y el resto de capas no se enterarán: solo cambia esta clase.
    //
    // Fíjate en algo importante: esta clase está en Infrastructure, pero implementa
    // una interfaz que vive en Domain. Esa es la "Inversión de Dependencias" en
    // acción: Domain no depende de Infrastructure, sino al revés.
    // ─────────────────────────────────────────────────────────────────────────────
    public class ResidenteRepository : IResidenteRepository
    {
        // "static" para que la lista sobreviva entre peticiones HTTP mientras la
        // app está viva. En un caso real esto sería una base de datos.
        private static readonly List<Residente> _residentesDb = new();

        public void Add(Residente residente)
        {
            _residentesDb.Add(residente);
        }

        public void Update(Residente residente)
        {
            // Como la entidad ya viene modificada por referencia (mismo objeto),
            // en una lista en memoria no haría falta hacer nada. Pero mostramos
            // el patrón explícito para que se entienda qué pasaría con una BD real.
            var indice = _residentesDb.FindIndex(r => r.Id == residente.Id);
            if (indice >= 0)
                _residentesDb[indice] = residente;
        }

        public void Delete(Guid id)
        {
            _residentesDb.RemoveAll(r => r.Id == id);
        }

        public IEnumerable<Residente> GetAll()
        {
            return _residentesDb;
        }

        public Residente? GetById(Guid id)
        {
            return _residentesDb.FirstOrDefault(r => r.Id == id);
        }

        public Residente? GetByName(string nombre)
        {
            // Comparación case-insensitive para que "ana" encuentre a "Ana".
            return _residentesDb.FirstOrDefault(r =>
                r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }
    }
}
