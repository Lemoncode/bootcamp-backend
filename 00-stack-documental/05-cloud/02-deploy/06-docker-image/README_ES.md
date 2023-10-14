# 06 Docker image

En este ejemplo vamos a crear y ejecutar imágenes Docker.

Tomamos como punto de partida el ejemplo `04-manual-aws-deploy`.

# Pasos

Ejecutamos `npm install` para instalar los paquetes de los ejemplos anteriores:

```bash
cd front
npm install
```

Abrimos un segundo terminal:

```bash
cd back
npm install

```

Ahora podemos crear nuestra imagen custom, en este caso vamos a usar [la imagen de node](https://hub.docker.com/_/node), en concreto la versión la versión alpine de linux como base para crear la nuestra:

> La version Alpine es una versión muy ligera de Linux.

_./Dockerfile_

```Docker
FROM node:18-alpine
```

> Una herramienta que está muy bien para trabajar con Docker es la extensión de VSCode [Docker](https://code.visualstudio.com/docs/containers/overview), te pone un menú de contexto en tu docker file y sintaxis highlight.

Vamos a crear un directorio para copiar nuestra aplicación (dentro del contenedor)

_./Dockerfile_

```diff
FROM node:18-alpine
+ RUN mkdir -p /usr/app
+ WORKDIR /usr/app

```

> ¿ Qué hace `RUN`? Ejecuta comandos dentro del contenedor (en este caso le indicamos que cree un directorio).
>
> ¿Qué hace `WORKDIR`? Esto hace que todos los comando que ejecutemos a partir de éste se ejecuten en este path.

Antes de ponernos a copiar ficheros vamos a crear un fichero `.dockerignore` para evitar copiar ficheros innecesarios (variables de entorno, ficheros de tests, node_modules, carpetas temporales...), así evitamos añadir peso innecesario en el contenedor.

_./.dockerignore_

```
back/node_modules
back/dist
back/mongo-data
back/public
back/.editorconfig
back/.env
back/.env.example
back/.env.test
back/.gitignore
back/create-dev-env.sh
back/docker-compose.yml
back/globalConfig.json
back/jest-mongodb-config.js

front/node_modules
front/dist
front/.editorconfig
front/.gitignore

```

Vamos a copiar los ficheros de nuestra aplicación (fijate que _COPY_ copia desde el disco duro de nuestra máquina al contenedor).

Copy all files:

_./Dockerfile_

```diff
FROM node:18-alpine
RUN mkdir -p /usr/app
WORKDIR /usr/app

+ COPY ./back ./

```

Le especificamos que ejecute el comando `npm install` para instalar las dependencias y después haga un build de la aplicación (`npm run build`), todo ello ya dentro del contenedor.

Fíjate que usamos _npm ci_ en vez de _npm install_ para instalar las dependencias, esto es porque _npm ci_ no modifica el package-lock.json.

_./Dockerfile_

```diff
FROM node:18-alpine
RUN mkdir -p /usr/app
WORKDIR /usr/app

COPY ./back ./
+ RUN npm ci
+ RUN npm run build
```

¿Como podemos ejecutar esta imagen? Necesitamos hacer un `build` de nuestra imagen custom antes de ejecutarla para que sea accesible por un contenedor docker.

```bash
docker build -t book-store-app:1 .
```

> -t: Le damos un nombre a la imagen. Podemos usar `-t nombre:tag`
> Cómo tag le indicamos que es la versión 1 de esta aplicación.

Vamos a ejecutar esta imagen para ver como va:

```bash
docker images

docker run --name book-container -it book-store-app:1 sh
```

> El tag en este caso es opcional.
>
> Lo abrimos en modo interactivo (shell de linux) para poder ver el contenido de la carpeta.

> Podemos los ficheros del build dentro del contenedor: `cd ./dist && ls`

Ya que tenemos el build, podemos probar a ejecutar el servidor:

_./Dockerfile_

```diff
FROM node:18-alpine
RUN mkdir -p /usr/app
WORKDIR /usr/app

COPY ./back ./
RUN npm ci
RUN npm run build

+ ENV PORT=3000
+ ENV STATIC_FILES_PATH=./public
+ ENV API_MOCK=true
+ ENV AUTH_SECRET=MY_AUTH_SECRET

+ RUN apk update && apk add jq
+ RUN updatedImports="$(jq '.imports[]|=sub("./src"; "./dist")' ./package.json)" && echo "${updatedImports}" > ./package.json
+ CMD node dist/index

```

¿Qué hace `RUN apk update && apk add jq`? Aquí estamos ejecutando dos comandos en secuencia (los separamos por el `&&`), el primero actualiza los paquetes de linux y el segundo instala el paquete `jq` (una herramienta para trabajar con json).

¿Qué hace `updatedImports="$(jq...)` ? Aquí tenemos dos comandos:

- El primero ejecuta el comando `jq` para modificar el fichero `package.json` y cambiar las rutas de los imports de `./src` a `./dist`, el resultado lo almacenamos en la variable `updatedImports`.
- El segundo volcamos el contenido de la variable `updatedImports` al fichero `package.json`.

> [jq](https://jqlang.github.io/jq/) and [jq playground](https://jqplay.org/)

_¡Ala que de lío ! Pero si esto lo hacíamos de forma manual... Todos los pasos manuales que podamos automatizar mejor, menos errores y más tiempo para tomar café ;)._

Fíjate que en el último comando no hacemos un _RUN_ sino `CMD node dist/index`, aquí le estamos diciendo, este comando no lo ejecutes ahora (tiempo de creación de imagen), ejecútalo cuando arranque el contenedor.

[CMD VS ENTRYPOINT](https://docs.doppler.com/docs/dockerfile)

Y en el docker file podemos usar variables de entorno con `ENV` (no incluyas secretos en este fichero porque ese fichero lo vamos a subir a un repositorio de github).

Volvemos a hacer un build de la imagen:

```bash
docker build -t book-store-app:1 .
docker images

docker container rm book-container
docker image prune
```

> Hacemos un _prune_ porque al crear una imagen con el mismo tag (1) que la anterior que creamos crea una imagen <none> debido a que reemplaza la anterior.
>
> La Eliminamos con el comando `docker image prune`

Vamos a ejecutar el nuevo container:

```bash
docker ps -a
docker run --name book-container book-store-app:1
docker exec -it book-container sh
```

Vamos a abrir el navegador y probar con las URL `http://localhost:3000` y `http://localhost:3000/api/books`. ¡¡ EY, ESTO NO FUNCIONA !! ¿Porque no podemos ver estas URLS? Porque este proceso se está ejecuando dentro del contenedor, tenemos que exponerlos puertos del contenedor a nuestras máquinas.

Vamos a hacer esto primero por línea de comandos, le pondremos el puerto 3001 de nuestra máquina para ver que se pueden poner puertos distintos ¿Para que sirve esto? Imagínate que necesitas correr Mongo 5 y Mongo 7... pues puedes tener dos contenedores con Mongo corriendo en puertos distintos.

```bash
docker ps
docker stop book-container
docker container rm book-container

docker run --name book-container --rm -d -p 3001:3000 book-store-app:1

docker ps
```

> [Docker run options](https://docs.docker.com/engine/reference/commandline/run/)
>
> --rm: Elimina el contenedor de forma automática cuando se detiene. Aún así tenemos que usar `docker stop`.
>
> -d: Detach mode. Ejecuta el contenedor en background y muestra el ID del contenedor.
>
> -p: Nos sirve para mapear puertos del contenedor a los de nuestra máquina.

Si intentamos abrir el navegador, en `http://localhost:3001` y `http://localhost:3001/api/books`, vemos que funciona (recibimos un `401 Unauthorized`, es un endpoint securizado es lo esperado).

Un comando que da información a quien vaya a consumir este DockerFile es el comando `EPXOSE`, estamos indicando que el contenedor expone el puerto 3000 (es sólo informativo).

_./Dockerfile_

```diff
...

+ EXPOSE 3000
ENV PORT=3000
...

```

> La instrucción `EXPOSE` no publica realmente el puerto. Funciona como un tipo de documentación entre la persona que construye la imagen y la persona que ejecuta el contenedor, sobre qué puertos se pretenden publicar.

Si ejecutamos un `docker images` vemos que tenemos `dangling images`, esto se debe a que hemos usado el mismo tag para cada build, podemos eliminarlas usando `prune`

```bash
docker images
docker rm book-container
docker image prune
```

Por otro lado, si echamos un vistazo podemos ver que nuestra imagen pesa mucho `~326MB`, es buena idea poner a dieta nuestra imagenes ya que menos peso significa:

- Menos tiempo de descarga.
- Menos espacio en disco.
- Menos tiempo de despliegue (acuerdate que nos cobran por minuto de build).
- Menos que nos cobrarían en un servicio cloud.

Para ello vamos a utilizar [multi-stage builds](https://docs.docker.com/develop/develop-images/multistage-build/), así podemos tener imágenes más pequeñas, ya que sólo incluiremos lo necesario para ejecutar nuestra aplicación.

En este caso vamos a usar tres fases:

- Base: Instalamos las dependencias y hacemos el build de la aplicación.
- Front-build: Hacemos el build de la aplicación front.
- Back-build: Hacemos el build de la aplicación back.
- Release: Copiamos los ficheros de los builds anteriores y ejecutamos la aplicación.

Cuando tenemos todos los pasos:

- Partimos de la `base` y lo volcamos a `release`.
- Copiamos los ficheros justos y necesario

Así pues nos quitamos un montón de ficheros temporales y dependencias que no necesitamos en producción.

_./Dockerfile_

```diff
- FROM node:18-alpine
+ FROM node:18-alpine AS base
RUN mkdir -p /usr/app
WORKDIR /usr/app

+ # Build front app
+ FROM base AS front-build
+ COPY ./front ./
+ RUN npm ci
+ RUN npm run build

+ # Build back app
+ FROM base AS back-build
COPY ./back ./
RUN npm ci
RUN npm run build

+ # Release
+ FROM base AS release
+ COPY --from=front-build /usr/app/dist ./public
+ COPY --from=back-build /usr/app/dist ./
+ COPY ./back/package.json ./
+ RUN apk update && apk add jq
+ RUN updatedImports="$(jq '.imports[]|=sub("./src"; ".")' ./package.json)" && echo "${updatedImports}" > ./package.json
+ COPY ./back/package-lock.json ./
+ RUN npm ci --only=production

EXPOSE 3000
ENV PORT=3000
ENV STATIC_FILES_PATH=./public
ENV API_MOCK=true
ENV AUTH_SECRET=MY_AUTH_SECRET

- RUN apk update && apk add jq
- RUN updatedImports="$(jq '.imports[]|=sub("./src"; "./dist")' ./package.json)" && echo "${updatedImports}" > ./package.json
- CMD node dist/index
+ CMD node index

```

> Necesitamos reemplazar los NodeJS imports en el `package.json` de `./src` a `root path` porque estamos copiando la carpeta `back/dist` a la carpeta `root` del contenedor.
>
> [Más info acerca jq](https://jqlang.github.io/jq/) and [jq playground](https://jqplay.org/)

Vamos a ejecutarlo:

```bash
docker build -t book-store-app:2 .
docker images

docker run --name book-container --rm -d -p 3001:3000 book-store-app:2

docker exec -it book-container sh

```

Open browser in `http://localhost:3001`

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
