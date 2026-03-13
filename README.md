# E-commerce Application

Full‑stack e‑commerce project with:

- **Backend:** ASP.NET Core **.NET 8** Web API (Clean Architecture style: API / Core / Infrastructure)
- **Frontend:** **Angular 15** client app
- **Integrations:** **Stripe** payments, **Redis** for caching, JWT-based authentication

> Repo layout:
>
> - `Client/ecommerceClint` → Angular app
> - `Server` → .NET solution (`Server.sln`) containing API/Core/Infrastructure projects

---

## Tech Stack

### Client (Angular)
- Angular `15.2.x`
- Bootstrap + Bootswatch, Font Awesome
- ngx-bootstrap, ngx-spinner, ngx-toastr
- Stripe.js (`@stripe/stripe-js`)

### Server (.NET)
- ASP.NET Core Web API (`net8.0`)
- Entity Framework Core (SQLite)
- ASP.NET Core Identity (Identity DB)
- JWT Authentication
- Swagger / OpenAPI (Swashbuckle)
- Redis (StackExchange.Redis)
- Stripe (`Stripe.net`)

---

## Getting Started

### Prerequisites
- **Node.js + npm** (for Angular)
- **Angular CLI** (optional, but recommended)
- **.NET SDK 8**
- **Docker** (optional, for Redis via compose)

---

## Running the Backend (API)

### 1) Start Redis (optional but recommended)
From the `Server` directory:

```bash
docker-compose up --detach
```

This starts:
- `redis` on `6379`
- `redis-commander` UI on `http://localhost:8081` (default credentials are in `docker-compose.yml`)

### 2) Run the API
From the `Server` directory:

```bash
dotnet restore
dotnet run --project API
```

The API config uses SQLite db files (see `appsettings.json`), and the app runs EF migrations + seeders automatically at startup.

### Swagger
Swagger is enabled in development. Once running, open the Swagger UI at the API base URL (commonly something like `https://localhost:5001/swagger` depending on your launch profile).

---

## Running the Frontend (Angular)

From `Client/ecommerceClint`:

```bash
npm install
npm start
```

Angular dev server runs at:

- `http://localhost:4200/`

---

## Configuration

### Server configuration files
- `Server/API/appsettings.json`
- `Server/API/appsettings.Development.json`

Key settings included:

- `ConnectionStrings:DefaultConnection` (SQLite)
- `ConnectionStrings:IdentityConnection` (SQLite)
- `ConnectionStrings:Redis` (defaults to `localhost`)
- `StripeSettings` (PublishableKey, SecretKey, Webhook secret)
- `Token` (JWT signing key + issuer)

> Important security note:
> The repository currently contains Stripe keys and a JWT signing key in `appsettings.json`.
> For real usage, **rotate these secrets** and move them to **User Secrets / environment variables** (and remove them from git history if this repo is public).

### Redis
If you run Redis using Docker Compose, your server can keep using:

- `ConnectionStrings:Redis = "localhost"`

If Redis is not running, some features that depend on it may fail (depending on how caching is used in the code).

---

## Database & Migrations

This solution uses EF Core with SQLite (based on `appsettings.json` connection strings).

Helpful commands (also noted in `Server/Notes`):

```bash
dotnet ef migrations add <MigrationName> -p Infrastructure -s API -c StoreContext
dotnet ef database update -p Infrastructure -s API -c StoreContext
```

Identity context example:

```bash
dotnet ef database update IdentityInitial -p Infrastructure -s API -c AppIdentityDbContext
```

---

## Project Structure (Server)

- `Server/API`
  - Controllers, DTOs, Middleware, Extensions
  - `Program.cs` configures services, middleware, CORS, auth, swagger, static file hosting, and applies migrations + seeding.
- `Server/Core`
  - Domain entities, interfaces, specifications
- `Server/Infrastructure`
  - EF Core data access, identity implementation, services (Redis/Stripe/etc.)

---

## Useful Notes

- Static files:
  - API serves static files and also exposes `Server/API/Content` under `/Content`
- CORS:
  - Enabled via a named policy: `"CorsPolicy"` (configured in service extensions)

---

## Scripts

### Client scripts (`Client/ecommerceClint/package.json`)
- `npm start` → `ng serve`
- `npm run build` → production build
- `npm test` → Karma unit tests

---

## Contributing

1. Fork the repo
2. Create a feature branch
3. Commit your changes
4. Open a pull request

---

## License

No license file was found in the repository. If you want others to use/modify this code, add a `LICENSE` file (e.g., MIT).
