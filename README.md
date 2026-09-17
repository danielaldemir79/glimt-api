# Glimt API

Det här är backenddelen till fotodagboken Glimt. API:t tar hand om minnesdata och sparar den i en lokal SQLite-databas.

## Teknik

- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger

## Starta backend

Du behöver ha .NET 10 SDK installerat.

Om datorn inte redan litar på .NET:s utvecklingscertifikat, kör följande kommando en gång:

```bash
dotnet dev-certs https --trust
```

Kommandot behövs för att webbläsaren och frontend ska kunna ansluta till det lokala API:t via HTTPS.

1. Öppna en terminal i backend repots rotmapp.

2. Hämta projektets paket:

   ```bash
   dotnet restore
   ```

3. Skapa databasen:

   ```bash
   dotnet ef database update
   ```

4. Starta API:t med HTTPS profilen:

   ```bash
   dotnet run --launch-profile https
   ```

5. Öppna Swagger i webbläsaren:

   ```text
   https://localhost:7092/swagger
   ```

Om `dotnet ef` inte finns installerat kan verktyget installeras med:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
```

## Endpoint

### Hämta alla minnen

```http
GET /api/MemoryEntries
```
Endpointen hämtar alla minnen från databasen och sorterar dem från nyast till äldst. Om databasen är tom returneras en tom lista.

### Skapa ett minne

```http
POST /api/MemoryEntries
```

POST tar emot ett minne som JSON, sparar det i databasen och returnerar det skapade minnet med status `201 Created`.

### Uppdatera ett minne

```http
PUT /api/MemoryEntries/{id}
```

PUT uppdaterar ett befintligt minne och returnerar det uppdaterade minnet. Om angivet id saknas returneras `404 Not Found`.

### Ta bort ett minne

```http
DELETE /api/MemoryEntries/{id}
```

DELETE tar bort ett befintligt minne och returnerar `204 No Content`. Om angivet id saknas returneras `404 Not Found`.

## Tekniska val

### HTTPS under utveckling

ASP.NET Core mallen skapade både en HTTP profil och en HTTPS profil för backend. Eftersom HTTP profilen ligger först väljer vi HTTPS profilen uttryckligen med `dotnet run --launch-profile https`. HTTPS krypterar trafiken mellan frontend och API. 

HTTPS profilen skyddar transporten men ger inte autentisering eller behörighetskontroll. I en produktionsmiljö hanteras HTTPS normalt av webbservern.

### SQLite

Vi valde SQLite eftersom projektet ska köras lokalt och inte behöver en separat databasserver. Det gör projektet enklare att starta efter kloning. Samtidigt får vi en riktig relationsdatabas som fungerar tillsammans med Entity Framework Core och migrations.

### Service och controller

Controllern hanterar HTTP anrop och returnerar HTTP-svar. Databaslogiken ligger i en separat service. Det håller controllern tunn och ger koden tydliga ansvarsområden.
