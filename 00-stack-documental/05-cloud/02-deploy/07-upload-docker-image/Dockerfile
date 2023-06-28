FROM node:18-alpine AS base
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
RUN npm ci --only=production

EXPOSE 3001
ENV PORT=3001
ENV STATIC_FILES_PATH=./public
ENV API_MOCK=true
ENV AUTH_SECRET=MY_AUTH_SECRET

CMD node index
