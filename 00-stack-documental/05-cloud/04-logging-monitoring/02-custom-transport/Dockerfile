FROM node:22-alpine AS base
RUN mkdir -p /usr/app
WORKDIR /usr/app

# Build front app
FROM base AS front-build
COPY ./front ./
RUN npm ci
RUN npm run build

# Build back app
FROM base AS back-build
COPY ./back ./
RUN npm ci
RUN npm run build

# Release
FROM base AS release
COPY --from=front-build /usr/app/dist ./public
COPY --from=back-build /usr/app/dist ./
COPY ./back/package.json ./
RUN apk update && apk add jq
RUN updatedImports="$(jq '.imports[]|=sub("./src"; ".")' ./package.json)" && echo "${updatedImports}" > ./package.json
COPY ./back/package-lock.json ./
RUN npm ci --omit=dev

ENV STATIC_FILES_PATH=./public
ENV IS_API_MOCK=false
ENV CORS_ORIGIN=false

CMD ["node", "index"]
