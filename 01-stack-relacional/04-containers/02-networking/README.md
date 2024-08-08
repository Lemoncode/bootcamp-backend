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

Ahora vamos a ver un ejemplo entre dos contenedores que utilizan una imagen con ping instalado. Para crear un contenedor con ping instalado, debes utilizar la imagen `networkstatic/fping`. Para crear un contenedor con esta imagen, debes ejecutar el siguiente comando:

```bash
docker run -d -it --name pepito networkstatic/fping
docker run -d -it --name jose networkstatic/fping
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

Ahora lo que vamos a ver es si pepito puede hacer ping a jose. Para hacer ping a jose desde pepito, debes ejecutar el siguiente comando:

```bash
docker exec pepito fping 172.17.0.3
```

Ahora lo que vamos a ver es si jose puede hacer ping a pepito. Para hacer ping a pepito desde jose, debes ejecutar el siguiente comando:

```bash
docker exec jose fping 172.17.0.2
```

Sin embargo, si pepito intenta hacer ping a jose utilizando su nombre, no va a poder. Para hacer ping a jose desde pepito utilizando su nombre, debes ejecutar el siguiente comando:

```bash
docker exec pepito fping jose
```

En la red brigde de Docker, los contenedores pueden comunicarse entre sí utilizando sus direcciones IP, pero no pueden comunicarse entre sí utilizando sus nombres. Para ello es necesario crear una red.


## Cómo crear una red

Para crear una red, debes utilizar el comando `docker network create`. Este comando recibe como parámetro el nombre de la red que quieres crear. Por ejemplo, si quieres crear una red llamada `lemon-network`, debes ejecutar el siguiente comando:

```bash
docker network create lemon-network
```

## Cómo conectar un contenedor a una red

Para conectar un contenedor a una red, debes utilizar la opción `--network` del comando `docker run`. Esta opción recibe como parámetro el nombre de la red a la cual quieres conectar el contenedor. Por ejemplo, si quieres conectar un contenedor a la red `lemon-network`, debes ejecutar el siguiente comando:

```bash
docker run --name don-pepito -it --network lemon-network networkstatic/fping
```

## Crear un segundo contenedor en la misma red y probar la conexión

Para crear un segundo contenedor en la misma red, debes utilizar el comando `docker run` con la opción `--network`. Por ejemplo, si quieres crear un segundo contenedor en la red `lemon-network`, debes ejecutar el siguiente comando:

```bash
docker run --name don-jose -it --network lemon-network networkstatic/fping

```

Para probar la conexión entre los contenedores, debes ejecutar el siguiente comando:

```bash
docker exec -it don-pepito ping don-jose
```

## Cómo desconectar un contenedor de una red

Para desconectar un contenedor de una red, debes utilizar el comando `docker network disconnect`. Este comando recibe como parámetros el nombre de la red y el nombre del contenedor. Por ejemplo, si quieres desconectar el contenedor `don-pepito` de la red `lemon-network`, debes ejecutar el siguiente comando:

```bash
docker network disconnect lemon-network don-pepito
```

## Cómo listar las redes

Para listar las redes, debes utilizar el comando `docker network ls`. Este comando no recibe ningún parámetro. Por ejemplo, si quieres listar las redes, debes ejecutar el siguiente comando:

```bash
docker network ls
```

## Cómo inspeccionar una red

Para inspeccionar una red, debes utilizar el comando `docker network inspect`. Este comando recibe como parámetro el nombre de la red que quieres inspeccionar. Por ejemplo, si quieres inspeccionar la red `lemon-network`, debes ejecutar el siguiente comando:

```bash
docker network inspect lemon-network
```

## Cómo eliminar una red

Para eliminar una red, debes utilizar el comando `docker network rm`. Este comando recibe como parámetro el nombre de la red que quieres eliminar. Por ejemplo, si quieres eliminar la red `lemon-network`, debes ejecutar el siguiente comando:

```bash
docker network rm lemon-network
```
