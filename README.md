# SolucionChida API

REST API construida con **.NET 9** que demuestra buenas prácticas de desarrollo backend: principios SOLID, arquitectura en capas, patrón Repository + Unit of Work y manejo de errores con el patrón Result.

---

## Tecnologías

| Tecnología | Uso |
|---|---|
| .NET 9 | Framework principal |
| Entity Framework Core | ORM |
| MySQL + Pomelo | Base de datos |
| FluentValidation | Validación de DTOs |
| OpenAPI (Scalar) | Documentación de endpoints |

---

## Arquitectura

```
SolucionChida/
├── Controllers/        # Capa de presentación — recibe y responde HTTP
├── Services/           # Lógica de negocio
├── Domain/
│   ├── Entities/       # Modelos del dominio (User, Role)
│   ├── DTOs/           # Objetos de transferencia de datos + validadores
│   └── Interfaces/     # Contratos de repositorios e infraestructura
├── Infrastructure/
│   ├── Data/           # DbContext + UnitOfWork
│   └── Repositories/   # Implementación de repositorios
└── Common/
    └── Result.cs       # Patrón Result para manejo de errores sin excepciones
```

---

## Endpoints — Users

| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/user` | Crear usuario |
| `GET` | `/api/user` | Listar todos los usuarios |
| `GET` | `/api/user/{id}` | Buscar usuario por ID |
| `GET` | `/email/{email}` | Buscar usuario por email |
| `PUT` | `/api/user/{id}` | Actualizar usuario |
| `DELETE` | `/api/user/{id}` | Eliminar usuario |

### Ejemplo — Crear usuario

**Request**
```json
POST /api/user
{
  "name": "Alex Millan",
  "email": "alex@example.com"
}
```

**Response `201 Created`**
```json
{
  "id": 1,
  "name": "Alex Millan",
  "email": "alex@example.com"
}
```

---

## Configuración

### 1. Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- MySQL 8+

### 2. Cadena de conexión

En `appsettings.json` o `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=solucionchida;User=root;Password=tu_password;"
  }
}
```

### 3. Aplicar migraciones

```bash
dotnet ef database update
```

### 4. Ejecutar el proyecto

```bash
dotnet run
```

La API estará disponible en `https://localhost:{puerto}`.  
La documentación OpenAPI en `https://localhost:{puerto}/openapi/v1.json`.

---

## Patrones implementados

**Repository Pattern** — abstrae el acceso a datos detrás de interfaces, facilitando pruebas y el intercambio de tecnología de persistencia.

**Unit of Work** — agrupa operaciones de base de datos en una sola transacción coherente.

**Result Pattern** — los servicios retornan `Result<T>` en lugar de lanzar excepciones, haciendo el flujo de éxito/error explícito en cada capa.

**DTOs + FluentValidation** — los datos de entrada nunca tocan las entidades directamente; son validados antes de llegar a la lógica de negocio.

---

## Autor

**Alex Millan** — [@AlexMillanG](https://github.com/AlexMillanG)
