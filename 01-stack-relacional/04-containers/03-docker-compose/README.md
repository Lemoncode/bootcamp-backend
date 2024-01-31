# Docker compose

## Qué es Docker compose

Docker compose es una herramienta que permite definir y ejecutar aplicaciones Docker multi-contenedor. Con Docker compose puedes definir la configuración de los contenedores de tu aplicación en un archivo YAML y luego ejecutarlos con un solo comando.

Para que puedas verlo con un ejemplo abre el archivo `docker-compose.yml` y revisa su contenido.

## Cómo ejecutar una aplicación Docker multi-contenedor con Docker compose

Para ejecutar una aplicación Docker multi-contenedor con Docker compose, debes utilizar el comando `docker-compose up`. Este comando recibe como parámetro el nombre del archivo YAML que contiene la configuración de los contenedores de tu aplicación. Por ejemplo, si quieres ejecutar la aplicación definida en el archivo `docker-compose.yml`, debes ejecutar el siguiente comando:

```bash
cd 01-stack-relacional/04-containers/03-docker-compose
docker-compose up -d
```

## Cómo detener una aplicación Docker multi-contenedor con Docker compose

Para detener una aplicación Docker multi-contenedor con Docker compose, debes utilizar el comando `docker-compose down`. Este comando recibe como parámetro el nombre del archivo YAML que contiene la configuración de los contenedores de tu aplicación. Por ejemplo, si quieres detener la aplicación definida en el archivo `docker-compose.yml`, debes ejecutar el siguiente comando:

```bash
docker-compose down
```



