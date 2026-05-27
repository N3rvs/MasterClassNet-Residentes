using Microsoft.AspNetCore.Mvc;
using Residentes.Application.Dtos;
using Residentes.Application.Services;

namespace Residentes.Api.Controllers
{
    // ─────────────────────────────────────────────────────────────────────────────
    // CONTROLADOR (capa de presentación / API)
    // ─────────────────────────────────────────────────────────────────────────────
    // El controlador es la PUERTA de entrada HTTP. Su trabajo es:
    //   1) Recibir la petición HTTP.
    //   2) Pasarle los datos al servicio de Application.
    //   3) Devolver un código HTTP apropiado (200, 201, 204, 400, 404...).
    //
    // Importante: el controlador NO contiene lógica de negocio. Si te pillas
    // poniendo `if (residente.Edad > 80)` aquí, párate: eso pertenece al dominio
    // o al servicio, NUNCA al controlador.
    // ─────────────────────────────────────────────────────────────────────────────
    [ApiController]
    [Route("api/[controller]")]
    public class ResidentesController : ControllerBase
    {
        private readonly ResidenteService _residenteService;

        // Inyección de Dependencias por constructor. Quien construye el
        // controlador (ASP.NET Core) nos pasa el servicio ya listo. Nosotros
        // no usamos `new ResidenteService(...)` aquí: eso lo gestiona el
        // contenedor de DI configurado en Program.cs.
        public ResidentesController(ResidenteService residenteService)
        {
            _residenteService = residenteService;
        }

        // ── GET /api/residentes ──────────────────────────────────────────────────
        // Lista todos los residentes.
        [HttpGet]
        public ActionResult<IEnumerable<ResidenteDto>> ObtenerTodos()
        {
            var residentes = _residenteService.ObtenerTodos();
            return Ok(residentes);
        }

        // ── GET /api/residentes/{id} ─────────────────────────────────────────────
        // Devuelve un residente concreto o 404 si no existe.
        [HttpGet("{id:guid}")]
        public ActionResult<ResidenteDto> ObtenerPorId(Guid id)
        {
            var residente = _residenteService.ObtenerPorId(id);
            if (residente is null)
                return NotFound($"No existe ningún residente con Id {id}.");

            return Ok(residente);
        }

        // ── POST /api/residentes ─────────────────────────────────────────────────
        // Crea un nuevo residente. Devuelve 201 Created con la URL del recurso.
        [HttpPost]
        public ActionResult<ResidenteDto> Crear([FromBody] CrearResidenteDto dto)
        {
            try
            {
                var creado = _residenteService.Crear(dto);

                // CreatedAtAction monta un 201 con la cabecera Location apuntando
                // al endpoint que permite leer ese recurso recién creado. Es la
                // forma "correcta" REST de responder a un POST.
                return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
            }
            catch (ArgumentException ex)
            {
                // Las validaciones del dominio lanzan ArgumentException.
                // Aquí las traducimos a un 400 BadRequest legible para el cliente.
                return BadRequest(ex.Message);
            }
        }

        // ── PUT /api/residentes/{id} ─────────────────────────────────────────────
        // Actualiza un residente existente. 204 si OK, 404 si no existía.
        [HttpPut("{id:guid}")]
        public IActionResult Actualizar(Guid id, [FromBody] ActualizarResidenteDto dto)
        {
            try
            {
                var actualizado = _residenteService.Actualizar(id, dto);
                if (!actualizado)
                    return NotFound($"No existe ningún residente con Id {id}.");

                // 204 NoContent es el código habitual para un PUT exitoso sin body.
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ── DELETE /api/residentes/{id} ──────────────────────────────────────────
        [HttpDelete("{id:guid}")]
        public IActionResult Eliminar(Guid id)
        {
            var eliminado = _residenteService.Eliminar(id);
            if (!eliminado)
                return NotFound($"No existe ningún residente con Id {id}.");

            return NoContent();
        }
    }
}
