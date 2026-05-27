# Mini-Gestor de Residentes

Proyecto **didáctico** en .NET 8 para aprender los fundamentos de una arquitectura
en capas tipo **DDD básico** sin librerías externas que tapen los conceptos.

La idea es entender PRIMERO el patrón a mano, y DESPUÉS sustituir cada pieza por
una librería estándar del ecosistema (MediatR, AutoMapper, FluentValidation, EF Core...).

---

## Estructura del repositorio

```
Mini-Gestor de Residentes/        → Proyecto Web API (capa de presentación)
├── Controllers/                  → Endpoints HTTP (solo hablan con Application)
└── Program.cs                    → Composición / Inyección de Dependencias

Residentes.Domain/                → Núcleo del negocio (no depende de NADIE)
├── Entities/Residente.cs         → Entidad con invariantes y comportamiento
└── Interfaces/                   → Contratos que el dominio NECESITA cumplir
    └── IResidenteRepository.cs

Residentes.Application/           → Casos de uso (depende solo de Domain)
├── Dtos/                         → Objetos de entrada/salida del API
└── Services/ResidenteService.cs  → Orquestación de operaciones

Residentes.Infraestructure/       → Detalles técnicos (depende de Domain)
└── Repositories/                 → Implementación concreta del repositorio
    └── ResidenteRepository.cs    → Persistencia en memoria (provisional)
```

### Regla de dependencias (importante)

```
Api  ──▶  Application  ──▶  Domain  ◀──  Infrastructure
```

- **Domain** no depende de nadie. Es puro C#.
- **Application** depende de Domain.
- **Infrastructure** depende de Domain (para implementar sus interfaces).
- **Api** los une a todos en `Program.cs`.

Si te pillas haciendo que Domain referencie a Application o Infrastructure, **PARA**:
eso rompe el diseño.

---

## Conceptos clave que se demuestran

| Concepto | Dónde verlo en el código |
|---|---|
| Entidad de dominio con invariantes | `Residentes.Domain/Entities/Residente.cs` |
| Setters privados + factory por constructor | mismo archivo |
| Comportamiento en la entidad (`ActualizarDatos`) | mismo archivo |
| Interfaz de repositorio en el Domain | `Residentes.Domain/Interfaces/IResidenteRepository.cs` |
| Inversión de Dependencias (D de SOLID) | la implementación está en Infrastructure |
| DTOs separados de la entidad | `Residentes.Application/Dtos/` |
| Mapeo manual entidad ↔ DTO | `ResidenteService.MapearADto` |
| Servicio de aplicación | `Residentes.Application/Services/ResidenteService.cs` |
| Inyección de dependencias en el contenedor | `Program.cs` |
| Controlador delgado, sin lógica de negocio | `Controllers/ResidentesController/ResidentesController.cs` |
| CRUD REST con códigos HTTP correctos (200/201/204/400/404) | mismo controlador |

---

## Endpoints disponibles

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/residentes` | Lista todos los residentes |
| GET | `/api/residentes/{id}` | Obtiene un residente por Id |
| POST | `/api/residentes` | Crea un residente |
| PUT | `/api/residentes/{id}` | Actualiza un residente |
| DELETE | `/api/residentes/{id}` | Elimina un residente |

Hay ejemplos listos para usar en el archivo `.http` del proyecto Api.

---

## Cómo ejecutarlo

```bash
dotnet build
dotnet run --project "Mini-Gestor de Residentes"
```

Abre `https://localhost:xxxx/` para ver Swagger.

---

## 👥 Trabajo en equipo con Git (MUY importante)

Una parte clave de este aprendizaje **no es solo el código**: es aprender a
colaborar como se hace en cualquier empresa real. Antes de tocar nada, lee esto.

### Reglas del repo

- La rama **`main` es mía** (la del profesor). **NADIE escribe directamente en `main`.**
- Cada alumno trabaja **en su propia rama** (o en su propio fork).
- Los cambios entran a `main` **solo por Pull Request**, después de revisión.

### Flujo recomendado: Fork + Pull Request

Este es el flujo estándar en proyectos open source y en muchas empresas.

**1) Hacer un fork** del repositorio desde GitHub (botón "Fork" arriba a la derecha).
Eso te crea una copia tuya bajo tu cuenta.

**2) Clonar TU fork** a tu máquina:

```bash
git clone https://github.com/TU-USUARIO/MasterClassNet-Residentes.git
cd MasterClassNet-Residentes
```

**3) Añadir el repo original como `upstream`** para poder traer cambios míos:

```bash
git remote add upstream https://github.com/N3rvs/MasterClassNet-Residentes.git
git remote -v   # comprueba que ves "origin" (tu fork) y "upstream" (el mío)
```

**4) Crear una rama** para cada ejercicio o feature. **Nunca trabajes sobre `main`.**

```bash
git checkout main
git pull upstream main          # sincronizas con la versión más reciente del profesor
git checkout -b feature/nombre-corto-del-cambio
```

Convención de nombres de rama:
- `feature/...` → algo nuevo (`feature/value-object-edad`)
- `fix/...` → arreglo de un bug (`fix/get-by-name-case-sensitive`)
- `chore/...` → tareas menores (`chore/actualizar-readme`)

**5) Trabajar, commitear con mensajes claros:**

```bash
git add .
git commit -m "feat: añadir Value Object Edad con validación de rango"
```

Buenos mensajes de commit:
- ✅ `fix: corregir GetByName para que sea case-insensitive`
- ✅ `feat: añadir endpoint DELETE de residentes`
- ❌ `cambios`, `update`, `asdf`, `aaa`

**6) Subir tu rama a TU fork:**

```bash
git push origin feature/nombre-corto-del-cambio
```

**7) Abrir un Pull Request** desde GitHub:
- Base: `N3rvs/MasterClassNet-Residentes` rama `main`
- Compare: `TU-USUARIO/MasterClassNet-Residentes` rama `feature/...`
- Describe **qué hiciste y por qué**.

**8) Esperar revisión.** Es normal que pida cambios. No es personal: es así
como se aprende. Aplica los cambios en la misma rama y vuelve a hacer
`git add` + `commit` + `push`. El PR se actualiza solo.

### Mantener tu fork actualizado

Cuando yo suba cambios a `main`, tú debes traerlos a tu fork:

```bash
git checkout main
git pull upstream main          # trae los cambios míos
git push origin main            # los sube a tu fork
```

Y si estabas a mitad de un ejercicio en otra rama, **rebasa**:

```bash
git checkout feature/mi-rama
git rebase main
```

### Cosas que NUNCA debes hacer

- ❌ `git push --force` sobre `main`.
- ❌ Commitear directamente en `main`.
- ❌ Subir archivos generados (`bin/`, `obj/`, `.vs/`). Para eso está el `.gitignore`.
- ❌ Subir secretos (cadenas de conexión, claves de API) en `appsettings.json`.
- ❌ Mezclar 5 ejercicios distintos en un mismo PR. Una rama = una cosa.

### Comandos de Git que vas a usar a diario

| Comando | Para qué |
|---|---|
| `git status` | Ver qué archivos has cambiado |
| `git diff` | Ver QUÉ has cambiado en esos archivos |
| `git log --oneline -10` | Ver los últimos 10 commits |
| `git branch` | Listar tus ramas locales |
| `git checkout -b nombre` | Crear y cambiarte a una rama nueva |
| `git switch nombre` | Cambiar a una rama existente |
| `git pull upstream main` | Traer los cambios del repo original |
| `git push origin mi-rama` | Subir tu rama a tu fork |

> 💡 Si te pierdes con Git, **NO improvises** con `--force`. Pregunta. Git es
> recuperable casi siempre… si no le mientes con `--force`.

---

## 🎯 Roadmap de mejoras (ejercicios para escalar el proyecto)

Cada bloque está pensado como un **ejercicio progresivo**. Hazlos en orden:
cada uno asume que el anterior ya está hecho.

### Nivel 1 — Refuerzo de fundamentos

- [ ] **GetByName real**: añadir un endpoint `GET /api/residentes/buscar?nombre=ana`
  que use el método del repositorio.
- [ ] **Validaciones manuales más estrictas** en el DTO de entrada (por ejemplo,
  edad mínima, géneros permitidos vía `enum`).
- [ ] Convertir `Genero` en un **`enum`** en lugar de un string suelto.

### Nivel 2 — Value Objects y dominio más rico

- [ ] Crear un **Value Object** `NombreCompleto` (con validación dentro)
  y sustituir el `string Nombre` de la entidad.
- [ ] Añadir un Value Object `Edad` que encapsule el rango válido.
- [ ] Añadir el concepto de **agregado**: por ejemplo, un `Residente` con
  una colección de `EpisodiosMedicos`.

### Nivel 3 — Manejo de errores y resultados

- [ ] Sustituir el lanzamiento de excepciones por un **Result pattern**
  (`Result<T>` con `IsSuccess`, `Error`...).
- [ ] Implementar un **middleware global de manejo de excepciones**
  para no llenar los controladores de `try/catch`.

### Nivel 4 — Persistencia real

- [ ] Cambiar el repositorio en memoria por **Entity Framework Core**
  con SQL Server o SQLite.
- [ ] Añadir un `DbContext` en la carpeta `Infraestructure/Context/`.
- [ ] Configurar **migraciones** (`dotnet ef migrations add ...`).
- [ ] Pasar el registro de DI de `AddSingleton` a `AddScoped`.

### Nivel 5 — Librerías estándar del ecosistema

> Solo entra aquí cuando entiendas POR QUÉ existe cada una. La regla es:
> primero a mano, después la librería.

- [ ] Sustituir los mapeos manuales por **AutoMapper** o **Mapster**.
- [ ] Sustituir las validaciones manuales por **FluentValidation**.
- [ ] Introducir **MediatR** y reorganizar Application en **CQRS**:
  carpetas `Commands/` y `Queries/` con sus handlers.
- [ ] Añadir el patrón **Specification** para queries complejas.

### Nivel 6 — Calidad y pruebas

- [ ] Tests unitarios del dominio (xUnit + FluentAssertions).
- [ ] Tests de integración del controlador con `WebApplicationFactory`.
- [ ] Cobertura con **coverlet** y reporte en consola.

### Nivel 7 — Producción

- [ ] **Logging estructurado** con Serilog.
- [ ] **HealthChecks** (`/health`).
- [ ] **Versionado de API** (`/api/v1/...`).
- [ ] **Autenticación** con JWT.
- [ ] **Eventos de dominio** (al crear un Residente crítico, publicar un evento).
- [ ] Empaquetar con **Docker** y un `docker-compose.yml` que levante la app + BD.

---

## Notas para quien aprende

- No te saltes pasos. El valor de este repo es ver **cómo encajan las piezas**.
- Si una capa "no entiende" algo, probablemente es porque no debería entenderlo.
  Ese es el diseño funcionando, no un problema.
- Antes de añadir una librería, pregúntate: *"¿qué problema concreto me resuelve
  que ahora mismo me esté doliendo?"*. Si no te duele, no la metas todavía.
