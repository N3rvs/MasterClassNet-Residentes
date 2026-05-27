using Residentes.Application.Services;
using Residentes.Domain.Interfaces;
using Residentes.Infraestructure.Repositories;

// ─────────────────────────────────────────────────────────────────────────────
// PUNTO DE ENTRADA DE LA APLICACIÓN
// ─────────────────────────────────────────────────────────────────────────────
// Aquí "atamos" todo el sistema:
//   - Configuramos el contenedor de Inyección de Dependencias (DI).
//   - Le decimos a ASP.NET Core qué implementación usar para cada interfaz.
//   - Construimos el pipeline HTTP (middlewares).
//
// Este archivo es el ÚNICO sitio donde se mencionan las tres capas a la vez.
// Eso es bueno: significa que las capas no se conocen entre sí directamente,
// solo aquí se conectan.
// ─────────────────────────────────────────────────────────────────────────────

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── Registro de dependencias ────────────────────────────────────────────────
//
// Cuando alguien pida un IResidenteRepository, dame un ResidenteRepository.
// AddSingleton: una sola instancia para toda la vida de la app (encaja con
// nuestra lista estática en memoria). Cuando pases a EF Core, esto pasará a
// ser AddScoped (una instancia por petición HTTP).
builder.Services.AddSingleton<IResidenteRepository, ResidenteRepository>();

// El servicio de Application también se registra para que el controlador
// pueda recibirlo por constructor. AddScoped es el valor seguro por defecto.
builder.Services.AddScoped<ResidenteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Swagger en la raíz: https://localhost:xxxx/
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
