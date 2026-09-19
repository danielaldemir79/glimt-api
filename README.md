# Glimt API

Det här är backenddelen till fotodagboken Glimt. API:t hanterar minnesdata och hämtar, skapar, uppdaterar och tar bort minnen i en lokal SQLite-databas.

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

Om `dotnet ef` inte finns installerat, installera verktyget:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
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


## Endpoints

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

### Ladda upp en bild

```http
POST /api/Images
```

Endpointen tar emot en bild som `multipart/form-data`. JPG, JPEG, PNG och WEBP är tillåtna och bilden får vara högst 5 MB. Bilden sparas med ett unikt filnamn och endpointen returnerar bildens publika sökväg.

## Tekniska val

### HTTPS under utveckling

ASP.NET Core mallen skapade separata profiler för HTTP och HTTPS. Frontend anropar `https://localhost:7092`, därför startar vi API:t med `dotnet run --launch-profile https`. HTTPS krypterar trafiken mellan frontend och API.

### CORS

Frontend och backend körs på olika portar under utveckling. Därför används en CORS policy som tillåter anrop från frontend på `http://localhost:5173`. Policyn är begränsad till den adress som frontend använder.

### Swagger och API-dokumentation

Swagger används för att dokumentera och testa API:ets endpoints under utvecklingen. XML kommentarer i controllers visas i Swagger så att det blir tydligare vad varje endpoint gör och vilka svar den kan returnera.

### SQLite

Vi valde SQLite eftersom projektet ska köras lokalt och inte behöver en separat databasserver. Det gör projektet enklare att starta efter kloning. Samtidigt får vi en riktig relationsdatabas som fungerar tillsammans med Entity Framework Core och migrations.

### Lagring av bilder

Bildfilen sparas i `wwwroot/uploads`, medan databasen bara sparar bildens publika sökväg. Det håller databasen mindre och gör att webbläsaren kan hämta bilden som en statisk fil. Bilder får unika filnamn för att filer med samma ursprungliga namn inte ska skriva över varandra.

När ett minne raderas tas även den tillhörande bildfilen bort från `wwwroot/uploads`. Bildens filnamn hämtas från den sparade sökvägen och filen raderas först efter att minnet har tagits bort från databasen. Om minnet saknar bild eller om bildfilen redan är borttagen fortsätter raderingen utan fel.

### Service och controller

Controllern hanterar HTTP anrop och returnerar HTTP-svar. Databaslogiken ligger i en separat service. Det håller controllern tunn och ger koden tydliga ansvarsområden.
