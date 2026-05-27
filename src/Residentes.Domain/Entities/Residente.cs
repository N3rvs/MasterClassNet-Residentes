namespace Residentes.Domain.Entities
{
    // ─────────────────────────────────────────────────────────────────────────────
    // ENTIDAD DE DOMINIO
    // ─────────────────────────────────────────────────────────────────────────────
    // Esta clase vive en la capa Domain porque representa un concepto del negocio
    // (un "residente" del centro). El Domain NO conoce nada de HTTP, ni de bases
    // de datos, ni de JSON. Es el corazón puro del sistema.
    //
    // Reglas que estamos aplicando aquí:
    //  1) Los setters son privados → nadie de fuera puede "mutar" el objeto a su
    //     antojo. Si quieres cambiar el estado, lo haces a través de un método
    //     de la propia entidad (más adelante: ActualizarDatos, etc.).
    //  2) Las invariantes (reglas que SIEMPRE deben cumplirse) se validan en el
    //     constructor. Si los datos no son válidos, el objeto ni siquiera se crea.
    //     Así nunca tendremos un Residente "inválido" dando vueltas por el código.
    // ─────────────────────────────────────────────────────────────────────────────
    public class Residente
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public int Edad { get; private set; }
        public string Genero { get; private set; } = string.Empty;
        public bool NecesitaAsistenciaMedica { get; private set; }

        // Constructor sin parámetros (protegido) que necesitan algunos ORMs (EF Core).
        // De momento no lo usamos, pero conviene dejarlo preparado.
        protected Residente() { }

        public Residente(Guid id, string nombre, int edad, string genero, bool necesitaAsistenciaMedica)
        {
            // Validaciones manuales — más adelante esto se puede mover a un
            // Value Object o usar FluentValidation, pero por ahora lo hacemos a
            // mano para entender el concepto: el dominio se protege a sí mismo.
            if (id == Guid.Empty)
                throw new ArgumentException("El Id no puede estar vacío.", nameof(id));

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));

            if (edad < 0 || edad > 130)
                throw new ArgumentOutOfRangeException(nameof(edad), "Edad fuera de rango razonable.");

            if (string.IsNullOrWhiteSpace(genero))
                throw new ArgumentException("El género es obligatorio.", nameof(genero));

            Id = id;
            Nombre = nombre.Trim();
            Edad = edad;
            Genero = genero.Trim();
            NecesitaAsistenciaMedica = necesitaAsistenciaMedica;
        }

        // Comportamiento de la entidad: en DDD la entidad no es un saco de datos,
        // también expone las acciones válidas que se pueden hacer sobre ella.
        public void ActualizarDatos(string nombre, int edad, string genero, bool necesitaAsistenciaMedica)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
            if (edad < 0 || edad > 130)
                throw new ArgumentOutOfRangeException(nameof(edad));
            if (string.IsNullOrWhiteSpace(genero))
                throw new ArgumentException("El género es obligatorio.", nameof(genero));

            Nombre = nombre.Trim();
            Edad = edad;
            Genero = genero.Trim();
            NecesitaAsistenciaMedica = necesitaAsistenciaMedica;
        }
    }
}
