# Dockerizar una aplicación

## Qué es Dockerizar una aplicación

Dockerizar una aplicación es crear una imagen Docker que contenga la aplicación y sus dependencias.

## Antes de empezar

Antes de dockerizar tu aplicación te recomiendo que la pruebes antes 😙 En este laboratorio, tienes una aplicación en el directorio `app`, la cual puedes ejecutarla en tu local, si tienes Node.js instalado, lanzando estos tres comandos:

```bash
cd 01-stack-relacional/04-containers/00-dockerizar-una-app/app
npm install
npm start
```
Esto hará que la aplicación esté disponible a través de [http://localhost:3000/](http://localhost:3000/).

Ahora que ya has validado que tu aplicación funciona, vamos a dockerizarla 😙.

## Cómo Dockerizar una aplicación

Dentro del mismo directorio `app` solo tienes que lanzar el comando `docker init` que te guiará a través de un asistente para crear una imagen Docker de tu aplicación.

```bash
docker init
```
Este te guiará a través de un asistente donde debes seleccionar:

1. Tecnología de la aplicación: `node`
2. Versión de la tecnología: `21.1.0`
3. Package manager: `npm`
4. El comando para ejecutar la aplicación: `npm start`
5. El puerto donde se ejecuta la aplicación: `3000`
6. Listo 🎉

Una vez que se haya generado el archivo `Dockerfile` ya puedes crear tu imagen. Para ello simplemente debes ejecutar el comando `docker build`. Este comando recibe como parámetro el nombre que quieres darle a la imagen y el directorio donde se encuentra el archivo `Dockerfile`. Por ejemplo, si quieres crear una imagen llamada `my-app` a partir del archivo `Dockerfile` que se encuentra en el directorio actual, debes ejecutar el siguiente comando:

```bash
docker build -t my-app .
```

El `.` significa qué path utiliza como contexto para la generación de la imagen. En este caso, el directorio actual.

## Cómo ejecutar una aplicación Dockerizada

Una vez que termine el proceso, ya puedes usar la misma para crear un contenedor que ejecute tu aplicación.

Solo debes utilizar el comando `docker run`. Este comando recibe como parámetro el nombre de la imagen que quieres ejecutar. Por ejemplo, si quieres ejecutar la imagen `my-app`, debes ejecutar el siguiente comando:

```bash
docker run -p 4000:3000 my-app 
```
Para que entiendas bien cómo funciona bien el parámetro `-p`, le he indicado que a través del puerto `4000` de mi máquina quiero exponer este contenedor que estoy creando, el cual está escuchando en el puerto `3000`.