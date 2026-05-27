namespace Residentes.Application.Dtos
{
    // ─────────────────────────────────────────────────────────────────────────────
    // DTO (Data Transfer Object)
    // ─────────────────────────────────────────────────────────────────────────────
    // ¿Por qué no devolvemos la entidad Residente directamente al cliente?
    //
    //  1) Para no EXPONER el dominio: la entidad tiene reglas, setters privados,
    //     comportamiento. Si la mandas tal cual por JSON, atas tu API a tu modelo
    //     interno y cualquier cambio en el dominio rompe a tus consumidores.
    //  2) Para controlar QUÉ se muestra: tal vez no quieras exponer ciertos
    //     campos sensibles. Con un DTO eliges exactamente qué viaja.
    //  3) Para tener un contrato estable hacia fuera, independiente del modelo
    //     interno.
    //
    // Usamos `record` porque queremos un objeto inmutable y con igualdad por valor,
    // perfecto para transportar datos.
    // ─────────────────────────────────────────────────────────────────────────────
    public record ResidenteDto
    {
        public Guid Id { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public int Edad { get; init; }
        public string Genero { get; init; } = string.Empty;
        public bool NecesitaAsistenciaMedica { get; init; }
    }

    // DTO de entrada para crear/actualizar. Lo separamos del DTO de salida
    // porque las necesidades son distintas (al crear no mandas Id, por ejemplo).
    public record CrearResidenteDto
    {
        public string Nombre { get; init; } = string.Empty;
        public int Edad { get; init; }
        public string Genero { get; init; } = string.Empty;
        public bool NecesitaAsistenciaMedica { get; init; }
    }

    public record ActualizarResidenteDto
    {
        public string Nombre { get; init; } = string.Empty;
        public int Edad { get; init; }
        public string Genero { get; init; } = string.Empty;
        public bool NecesitaAsistenciaMedica { get; init; }
    }
}
