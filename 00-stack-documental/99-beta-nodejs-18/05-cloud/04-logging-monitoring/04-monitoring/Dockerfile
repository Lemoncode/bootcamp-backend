FROM node:14-alpine AS base
RUN mkdir -p /usr/app
WORKDIR /usr/app

# Build front app
FROM base AS front-build
COPY ./front ./
RUN npm install
RUN npm run build

# Build back app
FROM base AS back-build
COPY ./back ./
RUN npm install
RUN npm run build

# Release
FROM base AS release
COPY --from=front-build /usr/app/dist ./public
COPY --from=back-build /usr/app/dist ./
COPY ./back/package.json ./
COPY ./back/package-lock.json ./
RUN npm ci --only=production

ENV NODE_ENV=production
ENV STATIC_FILES_PATH=./public
# Only for demo purpose
ENV API_MOCK=true
ENV AUTH_SECRET=MY_AUTH_SECRET

ENTRYPOINT ["node", "index"]
