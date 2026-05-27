using Residentes.Application.Dtos;
using Residentes.Domain.Entities;
using Residentes.Domain.Interfaces;

namespace Residentes.Application.Services
{
    // ─────────────────────────────────────────────────────────────────────────────
    // SERVICIO DE APLICACIÓN
    // ─────────────────────────────────────────────────────────────────────────────
    // La capa Application ORQUESTA los casos de uso del sistema:
    //   "crear un residente", "listarlos", "actualizar uno", "borrarlo".
    //
    // Reglas que estamos respetando:
    //  - El servicio depende de la INTERFAZ del repositorio (IResidenteRepository),
    //    NO de la implementación concreta. Esto se llama Inyección de Dependencias.
    //  - El servicio NO devuelve entidades de dominio al exterior: convierte a DTOs.
    //    Así el controlador (y por tanto, el mundo HTTP) nunca toca el dominio.
    //  - El mapeo entidad ↔ DTO lo hacemos a MANO a propósito. Más adelante puedes
    //    sustituirlo por AutoMapper o Mapster, pero primero hay que ENTENDER lo
    //    que está pasando: copiar campo a campo.
    // ─────────────────────────────────────────────────────────────────────────────
    public class ResidenteService
    {
        private readonly IResidenteRepository _repository;

        public ResidenteService(IResidenteRepository repository)
        {
            _repository = repository;
        }

        // ── CREATE ───────────────────────────────────────────────────────────────
        public ResidenteDto Crear(CrearResidenteDto dto)
        {
            // El Id lo genera el servidor, no el cliente. Patrón habitual.
            var nuevoId = Guid.NewGuid();

            // El constructor de la entidad validará las invariantes; si los datos
            // no son válidos, lanzará una excepción y el controlador la traducirá
            // en un 400 BadRequest.
            var residente = new Residente(
                nuevoId,
                dto.Nombre,
                dto.Edad,
                dto.Genero,
                dto.NecesitaAsistenciaMedica);

            _repository.Add(residente);

            return MapearADto(residente);
        }

        // ── READ ALL ─────────────────────────────────────────────────────────────
        public IEnumerable<ResidenteDto> ObtenerTodos()
        {
            return _repository.GetAll().Select(MapearADto).ToList();
        }

        // ── READ BY ID ───────────────────────────────────────────────────────────
        public ResidenteDto? ObtenerPorId(Guid id)
        {
            var residente = _repository.GetById(id);
            return residente is null ? null : MapearADto(residente);
        }

        // ── UPDATE ───────────────────────────────────────────────────────────────
        // Devolvemos bool: true si existía y se actualizó, false si no existe.
        // El controlador convertirá ese false en un 404 NotFound.
        public bool Actualizar(Guid id, ActualizarResidenteDto dto)
        {
            var residente = _repository.GetById(id);
            if (residente is null) return false;

            // La entidad sabe cómo mutarse de forma válida: nosotros no tocamos
            // sus propiedades una a una. Le pedimos que actualice sus datos
            // y ella se encarga de mantener sus invariantes.
            residente.ActualizarDatos(
                dto.Nombre,
                dto.Edad,
                dto.Genero,
                dto.NecesitaAsistenciaMedica);

            _repository.Update(residente);
            return true;
        }

        // ── DELETE ───────────────────────────────────────────────────────────────
        public bool Eliminar(Guid id)
        {
            var residente = _repository.GetById(id);
            if (residente is null) return false;

            _repository.Delete(id);
            return true;
        }

        // ── Helper privado de mapeo entidad → DTO ────────────────────────────────
        // En el roadmap, este método será reemplazado por AutoMapper/Mapster.
        private static ResidenteDto MapearADto(Residente r) => new()
        {
            Id = r.Id,
            Nombre = r.Nombre,
            Edad = r.Edad,
            Genero = r.Genero,
            NecesitaAsistenciaMedica = r.NecesitaAsistenciaMedica
        };
    }
}
