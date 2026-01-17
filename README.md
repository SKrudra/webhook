# webhook
Simple application with webhook publisher and consumer

---

## 🚀 Quick Start

Follow these steps to get the services and applications running locally.

### Prerequisites
- .NET SDK 10.0 (or the SDK matching the projects in the repo)
- Docker Desktop (Docker Engine + Docker Compose support)
- Git (for repository operations)

### Start infrastructure services (RabbitMQ + SQL Server)
A `docker-compose.yml` is provided at the repository root. It will start RabbitMQ (management UI) and SQL Server.

```powershell
# From the repo root
docker compose up -d
# Stop and remove containers
docker compose down
```

- RabbitMQ management UI: http://localhost:15672 (default credentials: `guest` / `guest`)
- SQL Server: listens on port `1433` (SA user). The `docker-compose.yml` uses `SA_PASSWORD=YourStrong!Passw0rd` — change that for production.

> ⚠️ If you already run SQL Server locally outside Docker, either remove the `sqlserver` service from `docker-compose.yml` or change ports/connection strings to avoid collisions.

### Configure connection strings and environment
Add or update `appsettings.json` or secrets for the projects that need DB and RabbitMQ connectivity. Example connection strings:

```json
// SQL Server
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=YourDatabase;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
}

// RabbitMQ (example)
"RabbitMq": {
  "Host": "localhost",
  "Port": 5672,
  "Username": "guest",
  "Password": "guest"
}
```

### Build the solution
From the repo root:

```powershell
dotnet build ./webhook.sln -c Release
```

Or build a single project:

```powershell
dotnet build ./AirlineWeb/AirlineWeb.csproj -c Release
```

### Run an application
Run a single project with `dotnet run`:

```powershell
# Example: run AirlineWeb
dotnet run --project ./AirlineWeb --configuration Debug
```

Or run from Visual Studio / VS Code (use the existing launch profiles in each project's `Properties/launchSettings.json`).

### Verify
- Open the RabbitMQ management UI at http://localhost:15672 and confirm queues/exchanges are available.
- Verify the application can connect to SQL Server and RabbitMQ by checking the application logs.

---

## 🗄️ Database migrations & seed data
The project uses EF Core. You can apply migrations automatically on app startup (the project calls `db.Database.Migrate()` in `StartUp.Configure`) or use the EF CLI to create and apply migrations manually.

1) Install EF tooling (if needed):
```powershell
# one-time (if dotnet-ef is not installed)
dotnet tool install --global dotnet-ef
```

2) Create a migration (example):
```powershell
# Creates migration files under AirlineWeb/Data/Migrations/WebhookInitialMigration
dotnet ef migrations add WebhookInitialMigration --project AirlineWeb --startup-project AirlineWeb --output-dir Data/Migrations/WebhookInitialMigration
```

3) Apply migrations to the database:
```powershell
# Applies pending migrations to the database specified by DefaultConnection
dotnet ef database update --project AirlineWeb --startup-project AirlineWeb
```

Alternative: run the app to apply migrations automatically (the app logs will show migration progress):
```powershell
# Run the web app which will apply migrations at startup
dotnet run --project ./AirlineWeb --configuration Debug
```

4) Verify tables exist (example using sqlcmd):
```powershell
sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -d AirlineDb -Q "SELECT name FROM sys.tables;"
```

Troubleshooting
- If you see errors like `mssql: An exception occurred while executing a Transact-SQL statement or batch`:
  - Confirm the `DefaultConnection` in `AirlineWeb/appsettings.Development.json` matches the SQL Server instance (port, SA password).
  - Ensure SQL Server container is healthy: `docker ps` and `docker logs sqlserver`.
  - Confirm your user has permissions and the database exists. `dotnet ef database update` will create the database if it does not exist.

Seeding example (C#)
- To add sample data after migrations, add this snippet to `StartUp.Configure` after `db.Database.Migrate()`:
```csharp
if (!db.WebhookSubscriptions.Any())
{
    db.WebhookSubscriptions.Add(new WebhookSubscription
    {
        WebhookUrl = "https://example.com/hook",
        Secret = "changeme",
        WebhookType = "sample.type",
        WebhookPublisher = "sample"
    });
    db.SaveChanges();
}
```

---

## 🌐 Testing REST APIs with HTTP Client Plugin

This project uses the REST Client extension (HTTP Client) to test REST APIs directly from VS Code.

### Prerequisites
- Install the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension by Huachao Mao in VS Code.

### Using AirlineWeb.http

The [AirlineWeb.http](AirlineWeb/AirlineWeb.http) file contains all REST API requests for the AirlineWeb service.

**How to use:**
1. Ensure AirlineWeb is running locally on `http://localhost:5052`
2. Open `AirlineWeb.http` in VS Code
3. Click the **Send Request** link that appears above each request to execute it
4. View the response in the **Response** panel on the right side

**Available Endpoints:**
- `POST /api/webhooksubscriptions` - Create a new webhook subscription
- `GET /api/webhooksubscriptions/{id}` - Get a webhook subscription by ID

### Response Variables
The HTTP file uses a variable `@AirlineWeb_HostAddress` that defaults to `http://localhost:5052`. You can modify this if your service runs on a different port.

---

## 🧰 Helpful Commands
- Bring up services: `docker compose up -d`
- Stop services: `docker compose down`
- Rebuild solution: `dotnet build ./webhook.sln -c Release`
- Run a project: `dotnet run --project ./<ProjectName>`

---

## 📝 Notes
- The `.gitignore` in the repo excludes generated build outputs and local secrets like `appsettings.Development.json`.
- Remember to change the SA password used in `docker-compose.yml` and in local `appsettings` before deploying anything to production.

If you'd like, I can add example environment files for each project or add step-by-step deployment scripts. ✅
