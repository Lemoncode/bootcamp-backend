# Introducción a contenedores

En esta sección se explicará el concepto de contenedores y se mostrará como crearlos y utilizarlos.

## Contenedores

Los contenedores son procesos que se ejecutan en un sistema operativo aislado. Este aislamiento se logra utilizando los siguientes recursos del sistema operativo:

- **Namespaces**: Permiten aislar los recursos del sistema operativo, como por ejemplo el sistema de archivos, las interfaces de red, los identificadores de procesos, etc.
- **Cgroups**: Permiten limitar los recursos que puede utilizar un proceso, como por ejemplo la cantidad de memoria, la cantidad de CPU, etc.
- **Chroot**: Permite cambiar el directorio raíz de un proceso.
- **Seccomp**: Permite limitar las llamadas al sistema que puede realizar un proceso.
- **Capabilities**: Permiten limitar los privilegios de un proceso.
- **AppArmor**: Permite limitar los recursos que puede utilizar un proceso.
- **SELinux**: Permite limitar los recursos que puede utilizar un proceso.
- **Kernel modules**: Permiten limitar los recursos que puede utilizar un proceso.
- **Linux Security Modules**: Permiten limitar los recursos que puede utilizar un proceso.

## Docker

Docker es una plataforma de código abierto que permite crear, ejecutar y compartir contenedores. 

### Instalación

La forma más sencilla de instalar Docker es utilizando Docker Desktop. Para instalarlo debes ir a esta [página](https://www.docker.com/products/docker-desktop/) y descargar el instalador correspondiente a tu sistema operativo.

## Crea tu primer contenedor

Una vez que hayas instalado Docker Desktop, puedes crear tu primer contenedor. Para ello, debes abrir una terminal y ejecutar el siguiente comando:

```bash
docker run hello-world
```

Este comando descargará la imagen `hello-world` desde Docker Hub y ejecutará un contenedor utilizando dicha imagen. El contenedor mostrará un mensaje y luego se detendrá.

¡Felicidades 🎉! Acabas de crear tu primer contenedor.

## Crea tu primer contenedor con una aplicación web

Ahora que ya sabes como crear un contenedor, puedes crear un contenedor con una aplicación web. En este caso no solo basta con ejecutarlo, sino que seguramente querrás poder acceder a la misma. Para ello, debes abrir una terminal y ejecutar el siguiente comando:

```bash
docker run  -p 8080:80 nginx
```

Este comando descargará la imagen `nginx` desde Docker Hub y ejecutará un contenedor utilizando dicha imagen. El contenedor ejecutará un servidor web en el puerto `80` y lo expondrá en el puerto `8080` de tu máquina.

Para verificar que el contenedor se está ejecutando, debes abrir un navegador web y acceder a la URL `http://localhost:8080`.

En este caso, como ves, el terminal se queda enganchado a la ejecución de este contenedor en concreto. Para evitarlo, puedes ejecutar el contenedor en segundo plano, añadiendo el parámetro `-d` al comando anterior:

```bash
docker run -d -p 8080:80 nginx
```

Para ver los contenedores que tienes ejecutándose puedes ejecutar el siguiente comando:

```bash
docker ps
```

Si además quieres ver los contenedores que tienes parados, puedes ejecutar el siguiente comando:

```bash
docker ps -a
```

Por otro lado, si quieres eliminar un contenedor, puedes ejecutar el siguiente comando:

```bash
docker rm <CONTAINER_ID>
```

Si el contenedor no está parado no se puede eliminar, a no ser que lo fuerces con:

```bash
docker rm -f <CONTAINER_ID>
```

O lo pares antes con el comando:

```bash
docker stop <CONTAINER_ID>
```

En estas primeras demos has conseguido descargar dos imágenes para poder crear dos contenedores. Si quieres ver las imágenes que tienes descargadas puedes ejecutar el siguiente comando:

```bash
docker images
```

Si quieres eliminar una imagen, puedes ejecutar el siguiente comando:

```bash
docker rmi <IMAGE_ID>
```

¡Felicidades 🎉! Acabas de crear tu primer contenedor con una aplicación web.