# ShipTracker

For frontend go (https://github.com/eduard-negescu/shiptracker_angular)[here]
## Project Installation
Run inside ShipTracker.Server
```docker compose up -d``` (For creating a container for a PostgreSQL DB)
```dotnet restore```
```dotnet ef database update``` (For migrations)
```dotnet run --launch-profile https``` (The Angular Frontend works only with the https)

## APIs
See ```https://localhost:7167/swagger/``` for OpenAPI documentation.

## Technologies
* ASP.NET 8.0
* EF Core
* PostgreSQL
* NUnit, Moq