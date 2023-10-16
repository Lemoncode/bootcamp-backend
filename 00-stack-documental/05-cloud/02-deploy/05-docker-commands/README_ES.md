# 05 Docker commands

En este ejemplo vamos a familiarizarnos con comandos de Docker.

Primero, vamos a comprobar las imágenes disponibles en local:

```bash
docker images
```

¿Cómo descargar una nueva? Podemos usar `docker pull` para descargar imágenes pre-construidas externas. Si no usamos ningún tag, descargará la última versión:

```bash
docker pull hello-world
```

> `hello-world` es una imagen que esta publicada en un registry.
> Por defecto descarga todas las imágenes del [Docker hub](https://hub.docker.com/).
> [También tienes opción de hacer pull de otros registries](https://docs.docker.com/engine/reference/commandline/pull/#pull-from-a-different-registry): docker pull myregistry.local:5000/testing/test-image

Vamos a ver ahora que imágenes tenemos disponibls:

```bash
docker images
```

Para ejecutar una imagen necesitamos un `container` de Docker, para crear un nuevo container basado en una imagen, necesitamos usar el comando `docker run`. Antes de eso, podemos listar cuantos containers tenemos:

```bash
docker ps
docker ps --all
docker ps -a
```

> ps: Process Status
> `docker ps`: Listado de contenedores que tenemos activos
> `docker ps -a / --all`: Lista todos los containers (activos y no activos)

Vamos a ejecutar la imagen:

```bash
docker run hello-world
```

Esta `image` se ejecutó en un `container` y se paró. Podemos comprobarlo con:

```bash
docker ps -a
```

Para lanzar un container parado tenemos que usar `docker start` no `run` porque `docker run` creará un nuevo container a partir de la imagen:

```bash
docker start <Container ID> -i
```

> -i / --interactive: modo interactivo, aquí conectas la entrada estándar del container con la entrada estándar del terminal.
> No tenemos que teclar el código entero del container, con los 4 primeros dígitos nos puede valer.

Creando un nuevo container con la misma imagen:

```bash
docker run hello-world
docker ps -a
```

> NOTE: Podemos nombrar un container como: `docker run --name my-container hello-world`

Vamos a eliminar todos los containers que están parados.

```bash
docker container rm <CONTAINER ID>
docker rm <CONTAINER ID>
docker container prune
```

> `prune`: Con esto direcamente borramos todos los que están parados.

Docker run `pulls` se trae las imágenes de forma automática si no están ya en local, Vamos a eliminar la imagen actual:

```js
// Lista las imagenes que tenemos en local
docker images

// Sirve para eliminar una imagen
docker image rm <IMAGE ID>:<tag>

// comando corto
docker rmi <IMAGE ID>:<tag>

// Elimina todas las imágenes fantasma
docker image prune
```

> `prune`: Elimina todas las imagenes fantasma (`dangling`), esto es todas las imagenes con nombre igual a <none>. Las imagenes `dangling` no están referenciadas por otras imágenes y es seguro borrarlas.

Ahora si podemos ejecutar un container en modo interactivo:

```bash
docker run ubuntu // exited automatically
docker run -it ubuntu sh
```

> Descarga la imagen `ubuntu` sin usar el comando `pull`.
>
> Abre el terminal de ese `ubuntu`, prueba: a ejecutar el comando `ls`.
> NOTA: Abre un nuevo terminal y escribe `docker ps`.
>
> sh: bash terminal
>
> Para salir del terminal de ubuntu: `exit`
> En los siguientes ejemplos veremos como conectarnos a un container en ejecución usando `docker exec -it <Container ID> sh

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
