# Dockerizar una aplicación

## Qué es Dockerizar una aplicación

Dockerizar una aplicación es crear una imagen Docker que contenga la aplicación y sus dependencias.

## Cómo Dockerizar una aplicación

Ahora Dockerizar una aplicación es súper sencillo. Para este ejemplo accede a la carpeta `app` y revisa su contenido.

Una vez dentro puedes lanzar el comando `docker init` que te guiará a través de un asistente para crear una imagen Docker de tu aplicación.

```bash
docker init
```

Una vez que se haya generado el archivo `Dockerfile` ya puedes crear tu imagen. Para ello simplemente debes ejecutar el comando `docker build`. Este comando recibe como parámetro el nombre que quieres darle a la imagen y el directorio donde se encuentra el archivo `Dockerfile`. Por ejemplo, si quieres crear una imagen llamada `my-app` a partir del archivo `Dockerfile` que se encuentra en el directorio actual, debes ejecutar el siguiente comando:

```bash
docker build -t my-app .
```

## Cómo ejecutar una aplicación Dockerizada

Para ejecutar una aplicación Dockerizada, debes utilizar el comando `docker run`. Este comando recibe como parámetro el nombre de la imagen que quieres ejecutar. Por ejemplo, si quieres ejecutar la imagen `my-app`, debes ejecutar el siguiente comando:

```bash
docker run -p 3000:3000 my-app 
```
