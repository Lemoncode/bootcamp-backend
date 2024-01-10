# clean-architecture-soccer-efcore

Ejemplo de arquitectura limpia con modelo rico (no anémico) para un dominio de fútbol con equipos, partidos y goles. 

## Descripción
Soccer es una web API que permite añadir información sobre partidos de fútbol y permite recuperar reportes de partidos ya finalizados donde se muestra el marcador.

## Pre-requisitos
1. SDK de .NET 5.0. 
2. Un IDE de desarrollo .NET (e.g: JetBrains Rider, Visual Studio o Visual Studio Code)
3. Un servidor de SqlServer en ejecucución (e.g: contenedor para imagen SqlServer de docker)
	```
	docker run -e "ACCEPT_EULA=Y" \
	-e "SA_PASSWORD=Lem0nCode!" \
	-e "MSSQL_PID=Express" \
	-p 1433:1433 \
	--name sqlserver \
	-d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
	```
4. El Entity Framework Core CLI
	```
	dotnet tool install --global dotnet-ef
	```


## Migraciones
Para crear migraciones
```
dotnet ef migrations add inicial --project src/Lemoncode.Soccer.Infra.Repository.EfCore --startup-project src/Lemoncode.Soccer.WebApi
```
Para ejecutar migraciones
```
dotnet ef database update --project src/Lemoncode.Soccer.Infra.Repository.EfCore --startup-project src/Lemoncode.Soccer.WebApi
```

## Ejecución
```
 dotnet run --project src/Lemoncode.Soccer.WebApi
```
La aplicación web estará disponible con información Open API disponible en http://localhost:5001/swagger
