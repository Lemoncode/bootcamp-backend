# Arrancar SQL Server 

Antes que nada, necesitamos correr un servidor **SQL Server** en nuestro equipo. Aunque podríamos hacerlo de varias formas (incluso descargando e instalando **SQL Server** en nuestra máquina), vamos a utilizar una imagen de _docker_ que levante el servidor de **SQL Server** en nuestro local.

Las imágenes de _docker_ de diferentes versiones de **SQL Server** están disponibles en [este enlace](https://hub.docker.com/_/microsoft-mssql-server).

Para correr la imagen utilizaremos el siguiente comando de _docker_:

```bash
docker run -e "ACCEPT_EULA=Y" \
-e "SA_PASSWORD=Lem0nCode!" \
-e "MSSQL_PID=Express" \
-p 1433:1433 \
--name sqlserver \
-d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
```

> Hemos elegido la imagen _server:2017-latest-ubuntu_, una versión _Express_ de **SQL Server** suficiente para el objetivo de estos ejemplos.

Cuando ejecutamos este comando, _Docker Engine_ va a coger esta imagen de Docker Hub y la va a descargar en local. Este proceso, puede durar unos segundos hasta que se complete dependiendo del ancho de banda desde donde te estés conectando.

Una vez que la imagen se descarga y se genera el nuevo contenedor, a través del comando _docker ps_, podemos ver que está corriendo sin problemas:

```bash
$ docker ps
CONTAINER ID  IMAGE                              COMMAND        CREATED   STATUS  PORTS                    NAMES
6e1ec21035c9  mcr.../server:2017-latest-ubuntu   "/opt/ms..."   4 s       Up 3 s  0.0.0.0:1433->1433/tcp   sqlserver
```

## Primer contacto con SQL Server

Lo siguiente que vamos a hacer, ahora sí, es engancharnos a este contenedor para crear una base de datos dentro de este servidor, alguna tabla y algunos registros.

Para ello vamos a utilizar el comando _docker exec_ y enganchar este terminal con el contenedor anterior:

```bash
docker exec -it sqlserver bash
```

Una vez dentro del contenedor, ejecutamos la tool _sqlcmd_ que tenemos en _/opt/mssql-tools/bin_:

```bash
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Lem0nCode!
```

Vamos a crear nuestra base de datos:

```sql
CREATE DATABASE Lemoncode;
GO
```

Y seleccionarla:

```sql
USE Lemoncode;
GO
```

Creamos una tabla de ejemplo:

```sql
CREATE TABLE Cursos (Id INT, Nombre VARCHAR(50));
GO
```

Insertamos un par de registros:

```sql
INSERT INTO Cursos VALUES (1, 'Bootcamp Backend'), (2, 'Máster Frontend');
GO
```

Ahora podemos seleccionar todos los registros de nuestra tabla y ver los resultados:

```sql
1> SELECT * FROM Cursos;
2> GO
Id          Nombre
----------- --------------------------------------------------
          1 Bootcamp Backend
          2 Máster Frontend
```
