# clean-architecture-soccer

Ejemplo de arquitectura limpia con modelo rico (no anémico) para un dominio de fútbol con equipos, partidos y goles. Contiene tests unitarios y test funcionales que utilizan Test Server para enviar peticiones HTTP reales a los endpoint existentes.

## Descripción
Soccer es una web API que permite añadir información sobre partidos de fútbol y permite recuperar reportes de partidos ya finalizados donde se muestra el marcador.

## Pre-requisitos
1. SDK de .NET 5.0. 
2. Un IDE de desarrollo .NET (e.g: JetBrains Rider, Visual Studio o Visual Studio Code)

## Ejecución
```
 dotnet run --project src/Lemoncode.Soccer.WebApi
```
La aplicación web estará disponible con información Open API disponible en http://localhost:5000/swagger