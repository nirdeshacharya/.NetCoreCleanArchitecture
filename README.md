# .NetCoreCleanArchitecture

Clean architecture boilerplate for **.NET 9**. Minimum code to get you going fast. Identity (Roles, Users, etc.) and a user module are already implemented.

## Stack

- **.NET 9** — ASP.NET Core MVC
- **Entity Framework Core 9** — SQL Server
- **ASP.NET Core Identity** — authentication & role-based authorization
- **Bootstrap 5** + **jQuery 3** via CDN
- **Font Awesome 6** via CDN

## Architecture

```
src/
├── APP.Web/       # Presentation layer (controllers, views, models)
├── APP.Data/      # Domain layer (entities, DbContext, EF mappings)
├── APP.Repo/      # Data access layer (generic repository)
└── APP.Services/  # Business logic layer (services)
```

## Setup

1. Clone the repo
2. Install the EF Core CLI tool (once):
   ```bash
   dotnet tool install -g dotnet-ef
   ```
3. Update the connection string in `src/APP.Web/appsettings.json`
4. Run migrations from the `src/APP.Web/` directory:
   ```bash
   dotnet ef database update
   ```
5. Run the app:
   ```bash
   dotnet run --project src/APP.Web
   ```

Navigate to `http://localhost:65433` and you're good to go.

Make sure to look for comments on specific methods to understand their purpose.
