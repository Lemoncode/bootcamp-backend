# Docker y los datos

En muchos casos los contenedores no pueden ser totalmente stateless. Es decir, necesitan guardar ciertos datos para poder funcionar.

En esta sección te explicaré cómo usar bind mounts y volumes para persistir los datos de tus contenedores.

## Bind mounts

Los bind mounts son directorios o archivos que se montan en un contenedor. Estos directorios o archivos pueden estar en la máquina host o en un contenedor.

### Montar un directorio de la máquina host

Para montar un directorio de la máquina host, debes utilizar la opción `-v` o `--volume` del comando `docker run`. Esta opción recibe como parámetro el directorio que quieres montar en el contenedor. Por ejemplo, si quieres montar el directorio `html` en esta misma carpeta, debes ejecutar el siguiente comando:

```bash
cd 01-stack-relacional/04-containers/01-datos
docker run --name nginx -d -p 8080:80 -v $(pwd)/html:/usr/share/nginx/html nginx
```

Este comando ejecutará un contenedor utilizando la imagen `nginx`. El contenedor ejecutará un servidor web en el puerto `80` y lo expondrá en el puerto `8080` de tu máquina. Además, montará el directorio `html` en el directorio `/usr/share/nginx/html` del contenedor.

Para verificar que el contenedor se está ejecutando, debes abrir un navegador web y acceder a la URL `http://localhost:8080`.

### Montar un directorio de un contenedor

Para montar un directorio de un contenedor, debes utilizar la opción `--volumes-from` del comando `docker run`. Esta opción recibe como parámetro el nombre o el ID del contenedor del cual quieres montar el directorio. Por ejemplo, si quieres montar el directorio `/usr/share/nginx/html` del contenedor `nginx` en un nuevo contenedor, debes ejecutar el siguiente comando:

```bash
docker run --name nginx-mirror -d -p 9090:80 --volumes-from nginx nginx
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
docker run --name nginx-with-volume -d -p 7070:80 -v nginx-data:/usr/share/nginx/html nginx
```

Este comando ejecutará un contenedor utilizando la imagen `nginx`. El contenedor ejecutará un servidor web en el puerto `80` y lo expondrá en el puerto `7070` de tu máquina. Además, montará el volume `nginx-data` en el directorio `/usr/share/nginx/html` del contenedor.

Para verificar que el contenedor se está ejecutando, debes abrir un navegador web y acceder a la URL `http://localhost:7070`.

A través de la extensión de VS Code puedes acceder al path que está mapeado a este volumen y modificar cosas. Lo importante de este sistema es que además podemos compartir el volumen entre varios contenedores.

```bash
docker run --name nginx-with-same-volume -d -p 5050:80 -v nginx-data:/usr/share/nginx/html nginx
```

Puedes comprobar accediendo a la URL `http://localhost:5050` que el contenido es el mismo que en `http://localhost:7070`.

Y lo más importante: si eliminas el contenedor `nginx-with-volume` y vuelves a crearlo, el contenido del volumen se mantiene intacto.

La diferencia principal entre bind mount y los volumes es que los volumes se crean y administran desde Docker, mientras que los bind mounts se crean y administran desde el sistema operativo. Es decir, bind mount está fuertemente acoplado al sistema operativo, mientras que los volumes son independientes del sistema operativo.

## ¿Es posible migrar estos volumes a otro ordenador?

Sí, es posible migrar los volumes a otro ordenador. Para ello, debes utilizar el comando `docker volume inspect` para obtener el path del directorio que está mapeado al volume. Luego, debes copiar el contenido de ese directorio a otro ordenador y crear un volume con el mismo nombre. Por último, debes montar el volume en un contenedor.

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

>Si el volumen está en uso no te dejará eliminarlo

### Eliminar todos los volumes

Para eliminar todos los volumes, debes utilizar el comando `docker volume prune`. Este comando no recibe ningún parámetro. Por ejemplo, si quieres eliminar todos los volumes que tienes en tu máquina, debes ejecutar el siguiente comando:

```bash
docker volume prune
```

>Este solo eliminará los volumes que no estén en uso