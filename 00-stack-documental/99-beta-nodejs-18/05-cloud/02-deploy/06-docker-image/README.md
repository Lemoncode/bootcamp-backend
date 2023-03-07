# 06 Docker image

In this example we are going to create and run Docker images.

We will start from `04-manual-aws-deploy`.

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

We can create our custom images. In this case, we will use [the node image](https://hub.docker.com/_/node), the alpine version as base image to create our custom one:

_./Dockerfile_

```Docker
FROM node:18-alpine
```

> You can use [Docker VSCode extension](https://code.visualstudio.com/docs/containers/overview)

Let's create the path where we are going to copy our app:

_./Dockerfile_

```diff
FROM node:18-alpine
+ RUN mkdir -p /usr/app
+ WORKDIR /usr/app

```

> RUN: run commands inside container
>
> WORKDIR: all commands after that will be executed in this path

Let's add the `.dockerignore` to avoid unnecessary files:

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

Copy all files:

_./Dockerfile_

```diff
FROM node:18-alpine
RUN mkdir -p /usr/app
WORKDIR /usr/app

+ COPY ./back ./

```

Execute install and build:

_./Dockerfile_

```diff
FROM node:18-alpine
RUN mkdir -p /usr/app
WORKDIR /usr/app

COPY ./back ./
+ RUN npm ci
+ RUN npm run build

```

How we can run this image? We need to `build` our custom image before run it to be accesible by a docker container.

```bash
docker build -t book-store-app:1 .
```

> -t: Give a name to image. We can use `-t name:tag`

Let's run this image to check how it's going on:

```bash
docker images

docker run --name book-container -it book-store-app:1 sh
```

> Tag is optional.
>
> We can see the files after build in `cd ./dist && ls`

We could run this server after build it:

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

+ CMD node dist/index

```

> RUN vs ENTRYPOINT: I don't want to run `node server` when we build the image, we want to run it when run the container.
>
> We can provide env variables with `ENV` (not include secrets in this file because we will upload to github repository).

Build image again:

```bash
docker build -t book-store-app:1 .
docker images

docker container rm book-container
docker image prune

```
> It creates a <none> image due to replace same tag.
> We can remove it with `docker image prune`

Run new container:

```bash
docker ps -a
docker run --name book-container book-store-app:1
docker exec -it book-container sh
```

Open browser in `http://localhost:3000` and `http://localhost:3000/api/books`. Why can't we access to these URLs? Because this process is executing itself inside container, we need to expose to our machine:

```bash
docker ps
docker stop book-container
docker container rm book-container

docker run --name book-container --rm -d -p 3001:3000 book-store-app:1

docker ps
```

> [Docker run options](https://docs.docker.com/engine/reference/commandline/run/)
> --rm: Automatically remove the container when it exits. We still have to use `docker stop`.
> -d: Detach mode. Run container in background and print container ID
> -p: Expose a port or a range of ports

Open browser in `http://localhost:3001` and `http://localhost:3001/api/books`.

> It's working because we receive 401 Unauthorized

We could inform about how to running using the `EXPOSE` command (it's only to inform):

_./Dockerfile_

```diff
...

+ EXPOSE 3000
ENV PORT=3000
...

```

> The `EXPOSE` instruction does not actually publish the port. It functions as a type of documentation between the person who builds the image and the person who runs the container, about which ports are intended to be published.

If we check `docker images` we can see dangling images, due to use same tags for each build.

```bash
docker image prune
```

On the other hand, we have an image with `~271MB`, too much size isn't it?. We should use [multi-stage builds](https://docs.docker.com/develop/develop-images/multistage-build/) to decrease this size, with only the necessary info:

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
+ RUN sed -i 's/.\/dist/./' package.json
+ COPY ./back/package-lock.json ./
+ RUN npm ci --only=production

EXPOSE 3000
ENV PORT=3000
ENV STATIC_FILES_PATH=./public
ENV API_MOCK=true
ENV AUTH_SECRET=MY_AUTH_SECRET

- CMD node dist/index
+ CMD node index

```

> We need to replace NodeJS imports in `package.json` from `./dist` to `root path` becasue we are copying `back/dist` folder to the `root` one in the container.
>
> [More info about sed tool](https://www.gnu.org/software/sed/manual/sed.html)

Run it:

```bash
docker stop book-container

docker build -t book-store-app:2 .
docker images

docker run --name book-container --rm -d -p 3001:3000 book-store-app:2

docker exec -it book-container sh

```

Open browser in `http://localhost:3001`

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
