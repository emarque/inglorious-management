# Softball Manager API

The initial server is a .NET 10 Generic Host application using ASP.NET Core minimal APIs and MySQL through EF Core. It keeps the first domain foundation to seasons, teams, players, season rosters, separately tracked player-season payments, and scheduled games. A game can refer to a participating away team or a placeholder opponent name.

## Run locally

Install the .NET 10 SDK and have a MySQL 8.0.16+ server available. Configure the connection string through the environment; do not put credentials in source-controlled settings:

```sh
export ConnectionStrings__DefaultConnection='Server=localhost;Port=3306;Database=softball_manager;User=softball_app;Password=your-local-password'
export ASPNETCORE_URLS='http://0.0.0.0:8080'
dotnet tool install --global dotnet-ef --version 10.0.9
dotnet ef database update --project SoftballManager.Api
dotnet run --project SoftballManager.Api
```

Apply checked-in EF Core migrations explicitly before starting the service. `/health` is a liveness check, `/health/ready` checks MySQL connectivity, and `/api/seasons`, `/api/teams`, `/api/players`, and `/api/games` return the initial domain collections.

## Run on Linux

Install the .NET 10 runtime (or publish for Linux with `dotnet publish SoftballManager.Api -c Release -r linux-x64 --self-contained false`) and provide the same environment variables through the service manager or container. Bind `ASPNETCORE_URLS` to the desired interface and restrict database/network access according to the deployment environment.

## Migrations

The initial schema migration is stored in `SoftballManager.Api/Data/Migrations`. To create later migrations, install the .NET EF tool matching the project's EF Core version and run:

```sh
dotnet ef migrations add DescribeChange --project SoftballManager.Api
```
