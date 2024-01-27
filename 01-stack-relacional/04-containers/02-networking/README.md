# Docker y el networking

En esta sección, aprenderás a utilizar el networking de Docker para conectar contenedores entre sí y con el mundo exterior.

## Tipos de redes

Docker soporta los siguientes tipos de redes:

- **bridge**: Es la red por defecto. Los contenedores que se conectan a esta red pueden comunicarse entre sí.
- **host**: Los contenedores que se conectan a esta red pueden comunicarse con el mundo exterior.
- **overlay**: Los contenedores que se conectan a esta red pueden comunicarse con otros contenedores que se encuentran en diferentes máquinas.
- **macvlan**: Los contenedores que se conectan a esta red pueden comunicarse con el mundo exterior utilizando direcciones MAC.
- **none**: Los contenedores que se conectan a esta red no pueden comunicarse con ningún otro contenedor.

## Demo con la red por defecto

Cuando yo creo un contenedor, Docker lo conecta automáticamente a la red `bridge`. Para comprobarlo, ejecuta el siguiente comando:

```bash
docker run -d --name my-nginx nginx
```

Luego, ejecuta el siguiente comando para ver la lista de redes:

```bash
docker network ls
```

Verás que aparece una red llamada `bridge`. Para ver los detalles de esta red, ejecuta el siguiente comando:

```bash
docker network inspect bridge
```

Verás que aparece un objeto JSON con la información de la red. En este objeto, puedes ver que el contenedor `my-nginx` está conectado a la red `bridge`.

## Demo con la red host

Para crear un contenedor conectado a la red `host`, debes utilizar la opción `--network` del comando `docker run`. Esta opción recibe como parámetro el nombre de la red a la cual quieres conectar el contenedor. Por ejemplo, si quieres crear un contenedor conectado a la red `host`, debes ejecutar el siguiente comando:

```bash
docker run -d --name my-nginx-host --network host nginx
```

Inspecciona la red `host` y verás que el contenedor `my-nginx` está conectado a ella.

```bash
docker network inspect host
```

Si estás usando Docker Desktop no podrás acceder al contenedor `my-nginx-host` desde tu máquina porque Docker Desktop utiliza una máquina virtual para ejecutar los contenedores. Sin embargo, si estás usando Docker en Linux, puedes acceder al contenedor `my-nginx-host` desde tu máquina utilizando la dirección `localhost`.

```bash
curl localhost
```

## Cómo se hablan dos contenedores en la red bridge

Ahora vamos a ver un ejemplo entre dos contenedores que utilizan una imagen con ping instalado. Para crear un contenedor con ping instalado, debes utilizar la imagen `networkstatic/ip-tools`. Para crear un contenedor con esta imagen, debes ejecutar el siguiente comando:

```bash
docker run -d --name pepito networkstatic/ip-tools
docker run -d --name jose networkstatic/ip-tools
```

Ahora vamos a inspeccionar la red `bridge` para ver los detalles de los contenedores `pepito` y `jose`. Para inspeccionar la red `bridge`, debes ejecutar el siguiente comando:

```bash
docker network inspect bridge
```

Lo siguiente que vamos a hacer es ejecutar el comando `ip addr` en el contenedor `pepito` para ver su dirección IP. Para ejecutar este comando en el contenedor `pepito`, debes ejecutar el siguiente comando:

```bash
docker exec pepito ip addr
```

Lo siguiente que vamos a hacer es ejecutar el comando `ip addr` en el contenedor `jose` para ver su dirección IP. Para ejecutar este comando en el contenedor `jose`, debes ejecutar el siguiente comando:

```bash
docker exec jose ip addr
```



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
