# 07 Upload docker image

In this example we are going to upload Docker images to Dockerhub.

We will start from `06-docker-image`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd front
npm install

```

In a second terminal:

```bash
cd back
npm install

```

First, we need to login in Docker Hub registry:

```bash
docker login
docker login <registry>
```

> `<registry>`: By default is docker.io.
> We can use `docker info` to see it.

Then we need tag the image with registry and path information to match with the `DockerHub` repository name that we would like to upload:

```bash
docker tag <app-name>:<tag> <registry>/<path-to-repository>

# Docker Hub case
docker tag book-store-app:2 <user-name>/<app-name>

```

> `<registry>`: By default is docker.io.
> `<path-to-repository>`: In the DockerHub case is <user-name>/<app-name>
> `<tag>`: is optionally, by default would be latest.

Check image list again:

```bash
docker images
```

Now, we can use docker `push` to upload it:

```bash
docker push <user-name>/<app-name>
```

We can use same image to tag `DockerHub` versions:

```bash
docker tag <app-name>:<tag> <registry>/<path-to-repository>:<tag>

# Docker Hub case
docker tag book-store-app:2 <user-name>/<app-name>:<tag>

```

```bash
docker tag book-store-app:2 <user-name>/<app-name>:2
docker images
docker push <user-name>/<app-name>:2
```

Let's update the version:

_./Dockerfile_

```diff
...

- EXPOSE 3000
- ENV PORT=3000
+ EXPOSE 3001
+ ENV PORT=3001
...

```

Build and upload again:

```bash
docker build -t <user-name>/<app-name>:3 .
docker images
docker push <user-name>/<app-name>:3
```

We should update the `latest` version to tag equals `3`:

```bash
docker tag <user-name>/<app-name>:3 <user-name>/<app-name>
docker images
docker push <user-name>/<app-name>
```

> `latest` version doesn't upload automatically

We could remove all local images:

```bash
docker image rm book-store-app:1 book-store-app:2 <user-name>/<app-name>:2 <user-name>/<app-name>:3 <user-name>/<app-name>:latest

docker images
```

And create a container from Dockerhub's image:

```bash
docker run --name book-container --rm -d -p 3001:3001 <user-name>/<app-name>:3
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
