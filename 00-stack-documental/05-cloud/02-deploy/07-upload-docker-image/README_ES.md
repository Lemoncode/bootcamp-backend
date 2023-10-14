# 07 Subir una imagen de docker

En este ejemplo vamos a subir una imagin de Docker a `Dockerhub` (el registry oficial de Docker).

Tomamos como punto de partida `06-docker-image`.

# Pasos

Lo primero, necesitamos hacer login en el registry de Docker Hub:

```bash
docker login
docker login <registry>
```

> `<registry>`: Por defecto el registry al que apunta es `docker.io`
> pues usar el comando `docker info` para ver entre otra información a que registry estamos conectados.

Ahora teneos que taggear la imagen con la informacíon del registry y el path para que coincida con el nombre del repositorio de `DockerHub` que queremos subir:

```bash
docker tag <app-name>:<tag> <registry>/<path-to-repository>
```

En nuestro caso

```bash
docker tag book-store-app:2 <user-name>/<app-name>

```

> `<registry>`: Por defecto es docker.io.
> `<path-to-repository>`: En DockerHub es nombreusuario/nombreapp <user-name>/<app-name> > `<tag>`: Es opcional, si no lo informamos, el tag será _latest_.

Vamos a ver las imágenes que tenemos:

```bash
docker images
```

Y hacemos un docker `push` de la imagen para subirla:

```bash
docker push <user-name>/<app-name>
```

Podemos usar la misma imagen para hacer un tag en `DockerHub` con el tag `2`:

```bash
docker tag <app-name>:<tag> <registry>/<path-to-repository>:<tag>
```

En nuestro caso:

```bash
docker tag book-store-app:2 <user-name>/<app-name>:<tag>
```

La secuencia completa para subirla, sería:

```bash
docker tag book-store-app:2 <user-name>/<app-name>:2
docker images
docker push <user-name>/<app-name>:2
```

Vamos a cambiar el puerto para que correo en 3001

_./Dockerfile_

```diff
...

- EXPOSE 3000
- ENV PORT=3000
+ EXPOSE 3001
+ ENV PORT=3001
...

```

Hacemos un build y subimos de nuevo

```bash
docker build -t brauliodiez/book-store:3 .
docker images
docker push <user-name>/<app-name>:3
```

También podemos decir que `latest` apunte a la versión `3`:

```bash
docker tag <user-name>/<app-name>:3 <user-name>/<app-name>:latest
docker images
docker push <user-name>/<app-name>:latest
```

> Ojo que `latest` no se actualiza solo cuando subimos nueva versión.

Y ahora podemos eliminar las imágenes locales:

```bash
docker image rm book-store-app:1 book-store-app:2 <user-name>/<app-name>:2 <user-name>/<app-name>:3 <user-name>/<app-name>:latest

docker images
```

Y crear un contenedor desde la imagen de Dockerhub:

```bash
docker run --name book-container --rm -d -p 3001:3001 <user-name>/<app-name>:3
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
