# Orleans App PoC

## Opis projektu

OrleansApp to proof-of-concept aplikacji wykorzystującej Microsoft Orleans do budowy skalowalnych, rozproszonych systemów. Projekt ten demonstruje implementację architektury opartej na aktorach (Virtual Actor Pattern) w .NET 9, z podziałem na warstwy i wykorzystaniem najlepszych praktyk projektowych.

## Technologie

- **.NET 9**: Najnowsza wersja frameworka .NET
- **Microsoft Orleans 9.x**: Framework do budowy rozproszonych systemów
- **ASP.NET Core Minimal API**: Lekki sposób implementacji API
- **Serilog**: Zaawansowane logowanie z integracją z SEQ
- **Swagger/OpenAPI**: Dokumentacja API
- **Docker i Docker Compose**: Konteneryzacja dla łatwego wdrożenia

## Architektura

Projekt jest podzielony na kilka warstw zgodnie z zasadami czystej architektury:

- **OrleansApp.Api**: Warstwa prezentacji, zawiera Minimal API
- **OrleansApp.Application**: Warstwa aplikacji, zawiera logikę aplikacyjną
- **OrleansApp.Orleans.Interfaces**: Interfejsy ziaren Orleans
- **OrleansApp.Orleans.Grains**: Implementacje ziaren Orleans
- **OrleansApp.Domain**: Warstwa domeny, zawiera modele domeny
- **OrleansApp.Infrastructure**: Warstwa infrastruktury, dostęp do bazy danych
- **OrleansApp.Common**: Wspólne klasy i DTOs

## Funkcjonalności

- **Zarządzanie użytkownikami**: CRUD dla encji użytkowników
- **Orleans Dashboard**: Wizualizacja i monitorowanie ziaren Orleans
- **Globalna obsługa wyjątków**: Zunifikowane podejście do błędów
- **Logowanie**: Centralne logowanie do SEQ

## Uruchomienie projektu

### Wymagania

- .NET 9 SDK
- Docker i Docker Compose
- IDE (Visual Studio, VS Code, Rider)

### Uruchomienie lokalne

```bash
# Sklonuj repozytorium
git clone [url-repozytorium]
cd OrleansApp

# Przywróć pakiety NuGet
dotnet restore

# Uruchom aplikację
dotnet run --project OrleansApp.Api/OrleansApp.Api.csproj
