# Docker y el networking

En esta sección, aprenderás a utilizar el networking de Docker para conectar contenedores entre sí y con el mundo exterior.

## Tipos de redes

Docker soporta los siguientes tipos de redes:

- **bridge**: Es la red por defecto. Los contenedores que se conectan a esta red pueden comunicarse entre sí.
- **host**: Los contenedores que se conectan a esta red pueden comunicarse con el mundo exterior.
- **overlay**: Los contenedores que se conectan a esta red pueden comunicarse con otros contenedores que se encuentran en diferentes máquinas.
- **macvlan**: Los contenedores que se conectan a esta red pueden comunicarse con el mundo exterior utilizando direcciones MAC.
- **none**: Los contenedores que se conectan a esta red no pueden comunicarse con ningún otro contenedor.

## Cómo crear una red

Para crear una red, debes utilizar el comando `docker network create`. Este comando recibe como parámetro el nombre de la red que quieres crear. Por ejemplo, si quieres crear una red llamada `my-network`, debes ejecutar el siguiente comando:

```bash
docker network create my-network
```

## Cómo conectar un contenedor a una red

Para conectar un contenedor a una red, debes utilizar la opción `--network` del comando `docker run`. Esta opción recibe como parámetro el nombre de la red a la cual quieres conectar el contenedor. Por ejemplo, si quieres conectar un contenedor a la red `my-network`, debes ejecutar el siguiente comando:

```bash
docker run --network my-network nginx
```

## Crear un segundo contenedor en la misma red y probar la conexión

Para crear un segundo contenedor en la misma red, debes utilizar el comando `docker run` con la opción `--network`. Por ejemplo, si quieres crear un segundo contenedor en la red `my-network`, debes ejecutar el siguiente comando:

```bash
docker run --network my-network nginx
```

Para probar la conexión entre los contenedores, debes ejecutar el siguiente comando:

```bash
docker exec -it <CONTAINER_ID> ping <CONTAINER_NAME>
```

## Cómo desconectar un contenedor de una red

Para desconectar un contenedor de una red, debes utilizar el comando `docker network disconnect`. Este comando recibe como parámetros el nombre de la red y el nombre del contenedor. Por ejemplo, si quieres desconectar el contenedor `my-container` de la red `my-network`, debes ejecutar el siguiente comando:

```bash
docker network disconnect my-network my-container
```

## Cómo listar las redes

Para listar las redes, debes utilizar el comando `docker network ls`. Este comando no recibe ningún parámetro. Por ejemplo, si quieres listar las redes, debes ejecutar el siguiente comando:

```bash
docker network ls
```

## Cómo inspeccionar una red

Para inspeccionar una red, debes utilizar el comando `docker network inspect`. Este comando recibe como parámetro el nombre de la red que quieres inspeccionar. Por ejemplo, si quieres inspeccionar la red `my-network`, debes ejecutar el siguiente comando:

```bash
docker network inspect my-network
```

## Cómo eliminar una red

Para eliminar una red, debes utilizar el comando `docker network rm`. Este comando recibe como parámetro el nombre de la red que quieres eliminar. Por ejemplo, si quieres eliminar la red `my-network`, debes ejecutar el siguiente comando:

```bash
docker network rm my-network
```
