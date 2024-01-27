# Docker y los datos

En esta sección te explicaré cómo usar bind mounts y volumes para persistir los datos de tus contenedores.

## Bind mounts

Los bind mounts son directorios o archivos que se montan en un contenedor. Estos directorios o archivos pueden estar en la máquina host o en un contenedor.

### Montar un directorio de la máquina host

Para montar un directorio de la máquina host, debes utilizar la opción `-v` o `--volume` del comando `docker run`. Esta opción recibe como parámetro el directorio que quieres montar en el contenedor. Por ejemplo, si quieres montar el directorio `html` en esta misma carpeta, debes ejecutar el siguiente comando:

```bash
docker run -d -p 8080:80 -v $(pwd)/html:/usr/share/nginx/html nginx
```

Este comando ejecutará un contenedor utilizando la imagen `nginx`. El contenedor ejecutará un servidor web en el puerto `80` y lo expondrá en el puerto `8080` de tu máquina. Además, montará el directorio `html` en el directorio `/usr/share/nginx/html` del contenedor.

Para verificar que el contenedor se está ejecutando, debes abrir un navegador web y acceder a la URL `http://localhost:8080`.

### Montar un directorio de un contenedor

Para montar un directorio de un contenedor, debes utilizar la opción `--volumes-from` del comando `docker run`. Esta opción recibe como parámetro el nombre o el ID del contenedor del cual quieres montar el directorio. Por ejemplo, si quieres montar el directorio `/usr/share/nginx/html` del contenedor `nginx` en un nuevo contenedor, debes ejecutar el siguiente comando:

```bash
docker run -d -p 9090:80 --volumes-from nginx nginx
```

Este comando ejecutará un contenedor utilizando la imagen `nginx`. El contenedor ejecutará un servidor web en el puerto `80` y lo expondrá en el puerto `9090` de tu máquina. Además, montará el directorio `/usr/share/nginx/html` del contenedor `nginx` en el directorio `/usr/share/nginx/html` del nuevo contenedor.

Para verificar que el contenedor se está ejecutando, debes abrir un navegador web y acceder a la URL `http://localhost:9090`.

## Volumes

Los volumes se trata de un tipo de directorio que se monta en un contenedor. La diferencia entre un volume y un bind mount es que los volumes se crean y administran desde Docker, mientras que los bind mounts se crean y administran desde el sistema operativo.

### Crear un volume

Para crear un volume, debes utilizar el comando `docker volume create`. Este comando recibe como parámetro el nombre del volume que quieres crear. Por ejemplo, si quieres crear un volume llamado `nginx-data`, debes ejecutar el siguiente comando:

```bash
docker volume create nginx-data
```

### Montar un volume

Para montar un volume, debes utilizar la opción `-v` o `--volume` del comando `docker run`. Esta opción recibe como parámetro el nombre del volume que quieres montar en el contenedor. Por ejemplo, si quieres montar el volume `nginx-data` en el directorio `/usr/share/nginx/html` del contenedor, debes ejecutar el siguiente comando:

```bash
docker run -d -p 7070:80 -v nginx-data:/usr/share/nginx/html nginx
```

Este comando ejecutará un contenedor utilizando la imagen `nginx`. El contenedor ejecutará un servidor web en el puerto `80` y lo expondrá en el puerto `7070` de tu máquina. Además, montará el volume `nginx-data` en el directorio `/usr/share/nginx/html` del contenedor.

Para verificar que el contenedor se está ejecutando, debes abrir un navegador web y acceder a la URL `http://localhost:7070`.

### Montar un volume en un directorio de un contenedor


Para montar un volume en un directorio de un contenedor, debes utilizar la opción `--volumes-from` del comando `docker run`. Esta opción recibe como parámetro el nombre o el ID del contenedor del cual quieres montar el volume. Por ejemplo, si quieres montar el volume `nginx-data` del contenedor `nginx` en un nuevo contenedor, debes ejecutar el siguiente comando:

```bash
docker run -d -p 6060:80 --volumes-from nginx nginx
```

Este comando ejecutará un contenedor utilizando la imagen `nginx`. El contenedor ejecutará un servidor web en el puerto `80` y lo expondrá en el puerto `6060` de tu máquina. Además, montará el volume `nginx-data` del contenedor `nginx` en el directorio `/usr/share/nginx/html` del nuevo contenedor.

Para verificar que el contenedor se está ejecutando, debes abrir un navegador web y acceder a la URL `http://localhost:6060`.

### Listar los volumes

Para listar los volumes, debes utilizar el comando `docker volume ls`. Este comando no recibe ningún parámetro. Por ejemplo, si quieres listar los volumes que tienes en tu máquina, debes ejecutar el siguiente comando:

```bash
docker volume ls
```

### Inspeccionar un volume

Para inspeccionar un volume, debes utilizar el comando `docker volume inspect`. Este comando recibe como parámetro el nombre del volume que quieres inspeccionar. Por ejemplo, si quieres inspeccionar el volume `nginx-data`, debes ejecutar el siguiente comando:

```bash
docker volume inspect nginx-data
```

### Eliminar un volume

Para eliminar un volume, debes utilizar el comando `docker volume rm`. Este comando recibe como parámetro el nombre del volume que quieres eliminar. Por ejemplo, si quieres eliminar el volume `nginx-data`, debes ejecutar el siguiente comando:

```bash
docker volume rm nginx-data
```
