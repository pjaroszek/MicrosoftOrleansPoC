Orleans App PoC
Opis projektu
OrleansApp to proof-of-concept aplikacji wykorzystującej Microsoft Orleans do budowy skalowalnych, rozproszonych systemów. Projekt ten demonstruje implementację architektury opartej na aktorach (Virtual Actor Pattern) w .NET 9, z podziałem na warstwy i wykorzystaniem najlepszych praktyk projektowych.
Technologie

.NET 9: Najnowsza wersja frameworka .NET
Microsoft Orleans 9.x: Framework do budowy rozproszonych systemów
ASP.NET Core Minimal API: Lekki sposób implementacji API
Serilog: Zaawansowane logowanie z integracją z SEQ
Swagger/OpenAPI: Dokumentacja API
Docker i Docker Compose: Konteneryzacja dla łatwego wdrożenia

Architektura
Projekt jest podzielony na kilka warstw zgodnie z zasadami czystej architektury:

OrleansApp.Api: Warstwa prezentacji, zawiera Minimal API
OrleansApp.Application: Warstwa aplikacji, zawiera logikę aplikacyjną
OrleansApp.Orleans.Interfaces: Interfejsy ziaren Orleans
OrleansApp.Orleans.Grains: Implementacje ziaren Orleans
OrleansApp.Domain: Warstwa domeny, zawiera modele domeny
OrleansApp.Infrastructure: Warstwa infrastruktury, dostęp do bazy danych
OrleansApp.Common: Wspólne klasy i DTOs

Funkcjonalności

Zarządzanie użytkownikami: CRUD dla encji użytkowników
Orleans Dashboard: Wizualizacja i monitorowanie ziaren Orleans
Globalna obsługa wyjątków: Zunifikowane podejście do błędów
Logowanie: Centralne logowanie do SEQ

Uruchomienie projektu
Wymagania

.NET 9 SDK
Docker i Docker Compose
IDE (Visual Studio, VS Code, Rider)

Uruchomienie lokalne
bash# Sklonuj repozytorium
git clone [url-repozytorium]
cd OrleansApp

# Przywróć pakiety NuGet
dotnet restore

# Uruchom aplikację
dotnet run --project OrleansApp.Api/OrleansApp.Api.csproj
Uruchomienie przez Docker
bash# Zbuduj i uruchom kontenery
docker-compose up --build
Po uruchomieniu:

API będzie dostępne pod adresem: http://localhost:5080
Swagger UI będzie dostępne pod adresem: http://localhost:5080/swagger
Orleans Dashboard będzie dostępne pod adresem: http://localhost:5081

API Endpoints
Użytkownicy

GET /api/users/{id}: Pobierz użytkownika po ID
POST /api/users: Utwórz nowego użytkownika
PUT /api/users/{id}: Aktualizuj użytkownika
DELETE /api/users/{id}: Usuń użytkownika

Struktura ziaren Orleans
W projekcie zaimplementowano następujące ziarna:

UserGrain: Reprezentuje użytkownika i zarządza jego stanem

Rozszerzanie projektu
Dodawanie nowego ziarna

Zdefiniuj interfejs w projekcie OrleansApp.Orleans.Interfaces:

csharppublic interface IMyNewGrain : IGrainWithStringKey
{
    Task<ResultDto> MyOperationAsync(InputDto input);
}

Implementuj interfejs w projekcie OrleansApp.Orleans.Grains:

csharp[GenerateSerializer]
public class MyNewGrain : Grain, IMyNewGrain
{
    private readonly IPersistentState<MyState> _state;

    public MyNewGrain([PersistentState("state", "SqlStateStore")] IPersistentState<MyState> state)
    {
        _state = state;
    }

    public async Task<ResultDto> MyOperationAsync(InputDto input)
    {
        // Implementacja
    }
}

Dodaj endpoint w Program.cs:

csharpapp.MapPost("/api/mynew", async (InputDto input, IGrainFactory grainFactory) =>
{
    var grain = grainFactory.GetGrain<IMyNewGrain>(Guid.NewGuid().ToString());
    var result = await grain.MyOperationAsync(input);
    return Results.Ok(result);
});
Uwagi dotyczące wdrożenia produkcyjnego
Przed wdrożeniem na produkcję należy rozważyć:

Trwałość: Skonfigurowanie trwałego magazynu dla ziaren (SQL Server, MongoDB)
Klastrowanie: Konfiguracja klastra Orleans dla wysokiej dostępności
Bezpieczeństwo: Dodanie uwierzytelniania i autoryzacji
Monitorowanie: Rozszerzenie logowania i monitorowania
Skalowanie: Strategie skalowania dla obsługi dużego ruchu

Licencja
Ten projekt jest objęty licencją MIT. Szczegóły znajdują się w pliku LICENSE.

Projekt stworzony jako proof-of-concept i nie jest przeznaczony do bezpośredniego użycia w środowisku produkcyjnym bez odpowiednich modyfikacji.
