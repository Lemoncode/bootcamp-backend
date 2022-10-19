# sqlclient-soccer

Acceso a SqlServer utilizando SqlClient para un modelo E-R de fútbol con equipos, partidos y goles

## Instrucciones
Asegúrate de tener un servidor SqlServer disponible y que el hostname y las credenciales en `Program.cs` son correctas.

Compila con
```
dotnet build
```

Ejecuta con
```
cd src/Lemoncode.SqlClient.Soccer
dotnet run
```

## Anexo - SqlServer con docker (Anexo)
Crea un contenedor de SqlServer para tener un servidor de base de datos corriendo en local
```
docker run -e "ACCEPT_EULA=Y" \
-e "SA_PASSWORD=Lem0nCode!" \
-e "MSSQL_PID=Express" \
-p 1433:1433 \
--name sqlserver \
-d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
```

Asegúrate de que el contenedor extá en ejecución con
```
docker ps
```

Si ya tenías ese contenedor (u otro), puedes simplemente arrancarlo
```
docker start sqlserver
```