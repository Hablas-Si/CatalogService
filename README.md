# Catalog Service

Dette er en mikroservice til et auktionshus, der administrerer katalogdata.

## Beskrivelse

Catalog Service giver en RESTful API til at administrere katalogdata for auktionshusets varer. Det tillader oprettelse, læsning, opdatering og sletning af varer i kataloget.

## Funktioner

- Opret en ny vare i kataloget
- Hent en liste over alle varer i kataloget
- Hent en enkelt vare ved hjælp af dens ID
- Opdater en eksisterende vare i kataloget
- Slet en vare fra kataloget

## Teknologier

- **ASP.NET Core Web API**: Bruges til at oprette HTTP-tjenesten til håndtering af katalogoperationer.
- **C#**: Primærsproget til implementering af tjenestelogikken.
- **Entity Framework Core**: Bruges til dataadgang og til at arbejde med databasen.
- **Swagger**: Indbygget dokumentation og testværktøj til API'et.

## Installation

1. Sørg for at have .NET Core SDK installeret på din maskine.
2. Klon dette projekt til din lokale maskine.
3. Åbn projektet i din foretrukne IDE eller teksteditor.
4. Kør `dotnet run` fra rodmappen for at starte API'et.

## API-dokumentation

API'et er dokumenteret ved hjælp af Swagger, og du kan få adgang til dokumentationen ved at åbne følgende URL i din browser, når API'et kører:

https://localhost:{port}/swagger


Erstat `{port}` med den port, som API'et kører på (standardporten er normalt 5072 ellers port 4000 for NGINX).

# CatalogService# CatalogService
