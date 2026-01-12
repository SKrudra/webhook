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
