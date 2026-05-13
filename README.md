# SolucionChida API

REST API construida con **.NET 9** que demuestra buenas prácticas de desarrollo backend: principios SOLID, arquitectura en capas, patrón Repository + Unit of Work y manejo de errores con el patrón Result.

---

## Tecnologías

| Tecnología | Uso |
|---|---|
| .NET 9 | Framework principal |
| Entity Framework Core | ORM |
| MySQL + Pomelo | Base de datos |
| BCrypt.Net | Hash de contraseñas |
| JWT Bearer | Autenticación |
| FluentValidation | Validación de DTOs |
| OpenAPI (Scalar) | Documentación de endpoints |

---

## Arquitectura

```
SolucionChida/
├── Controllers/        # Capa de presentación — recibe y responde HTTP
├── Services/           # Lógica de negocio
├── Domain/
│   ├── Entities/       # Modelos del dominio (User, Role, Product, Category)
│   ├── DTOs/           # Objetos de transferencia de datos + validadores
│   └── Interfaces/     # Contratos de repositorios e infraestructura
├── Infrastructure/
│   ├── Data/           # DbContext + UnitOfWork
│   └── Repositories/   # Implementación de repositorios
└── Common/
    └── Result.cs       # Patrón Result para manejo de errores sin excepciones
```

---

## Endpoints

### Auth

| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/auth/login` | Iniciar sesión y obtener JWT |

**Request**
```json
POST /api/auth/login
{
  "email": "alex@example.com",
  "password": "secret123"
}
```

**Response `200 OK`**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

---

### Users

| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/user` | Crear usuario |
| `GET` | `/api/user` | Listar todos los usuarios |
| `GET` | `/api/user/{id}` | Buscar usuario por ID |
| `GET` | `/api/user/email/{email}` | Buscar usuario por email |
| `PUT` | `/api/user/{id}` | Actualizar usuario |
| `DELETE` | `/api/user/{id}` | Eliminar usuario |

**Request — Crear usuario**
```json
POST /api/user
{
  "name": "Alex Millan",
  "email": "alex@example.com",
  "password": "secret123",
  "rolesId": [1]
}
```

**Response `201 Created`**
```json
{
  "id": 1,
  "name": "Alex Millan",
  "email": "alex@example.com",
  "roles": [{ "id": 1, "name": "Admin" }]
}
```

---

### Categories

| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/category` | Crear categoría |
| `GET` | `/api/category` | Listar todas las categorías |
| `GET` | `/api/category/{id}` | Buscar categoría por ID |
| `GET` | `/api/category/name/{name}` | Buscar categoría por nombre |
| `PUT` | `/api/category/{id}` | Actualizar categoría |
| `DELETE` | `/api/category/{id}` | Eliminar categoría |

**Request — Crear categoría**
```json
POST /api/category
{
  "name": "Electrónica"
}
```

---

### Products

| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/product` | Crear producto |
| `GET` | `/api/product` | Listar todos los productos |
| `GET` | `/api/product/{id}` | Buscar producto por ID |
| `GET` | `/api/product/sku/{sku}` | Buscar producto por SKU |
| `GET` | `/api/product/category/{id}` | Listar productos por categoría |
| `PUT` | `/api/product/{id}` | Actualizar producto |
| `DELETE` | `/api/product/{id}` | Eliminar producto |

**Request — Crear producto**
```json
POST /api/product
{
  "name": "Laptop Pro",
  "description": "Laptop de alto rendimiento",
  "sku": "LAP-001",
  "categoryId": 1
}
```

**Response `200 OK`**
```json
{
  "id": 1,
  "name": "Laptop Pro",
  "description": "Laptop de alto rendimiento",
  "sku": "LAP-001",
  "categoryId": 1,
  "categoryName": "Electrónica"
}
```

---

## Configuración

### 1. Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- MySQL 8+

### 2. Variables de configuración

En `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=solucionchida;User=root;Password=tu_password;"
  },
  "Jwt": {
    "Key": "tu_clave_secreta_larga",
    "Issuer": "solucionchida",
    "Audience": "solucionchida"
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
La documentación interactiva (Scalar) en `https://localhost:{puerto}/scalar`.

---

## Patrones implementados

**Repository Pattern** — abstrae el acceso a datos detrás de interfaces, facilitando pruebas y el intercambio de tecnología de persistencia.

**Unit of Work** — agrupa operaciones de base de datos en una sola transacción coherente.

**Result Pattern** — los servicios retornan `Result<T>` en lugar de lanzar excepciones, haciendo el flujo de éxito/error explícito en cada capa.

**DTOs + FluentValidation** — los datos de entrada nunca tocan las entidades directamente; son validados antes de llegar a la lógica de negocio.

---

## Autor

**Alex Millan** — [@AlexMillanG](https://github.com/AlexMillanG)
