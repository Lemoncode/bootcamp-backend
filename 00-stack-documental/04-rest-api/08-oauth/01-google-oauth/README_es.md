# OAUTH2 + Google Passport + Cookie + JWT

En este ejemplo vamos a ver como autenticarnos usando el servicio OAuth2 de Google Cloud. Además, vamos a crear una cookie y token JWT para seguir autenticados.

## Pasos

Empezamos por copiarnos el ejemplo anterior _00-boilerplate_ e instalar las dependencias.

```bash
npm install
```

Y arrancamos el proyecto.

```bash
npm start
```

Al arrancar el proyecto se creará de forma automática un fichero `.env` con una serie de variables de entorno que la aplicación necesita para funcionar.

También podemos ver que hay dos endpoints interesantes. En uno se muestra una página, y en la otra tenemos un endpoint que devuelve un json:

- <http://localhost:3000> (página)
- <http://localhost:3000/api> (endpoint que devuelve un json)

Ahora vamos a parar la ejecución e instalar las dependencias que necesitamos, que en este caso son las siguientes:

- Vamos a trabajar con cookies (`cookie-parser`) y token jwt (`jsonwebtoken`) para guardar el id del usuario autenticado.
- Vamos a usar passport (`passport`) para gestionar la autenticación.
- Vamos a usar la estrategia de passport para gestionar la autenticación con Google (`passport-google-oauth20`).

```bash
npm install cookie-parser jsonwebtoken passport passport-google-oauth20
```

Y sus typings:

```bash
npm install -D @types/jsonwebtoken @types/passport @types/passport-google-oauth20
```

Ahora vamos a añadir dos nuevas variables de entorno para poder comunicarnos con Google. **No mostrar esto por pantalla, ni almacenar**

_./env_:

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
+ GOOGLE_CLIENT_ID= <pega aquí tu client Id de tu panel de Google accounts>
+ GOOGLE_CLIENT_SECRET= <pega aquí tu client Secret de tu panel de Google accounts>
```

Y añadimos los nuevos valores al _env.constants.ts_

_./src/env.constants.ts_:

```diff
export const envConstants = {
  NODE_ENV: process.env.NODE_ENV,
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
+ GOOGLE_CLIENT_ID: process.env.GOOGLE_CLIENT_ID,
+ GOOGLE_CLIENT_SECRET: process.env.GOOGLE_CLIENT_SECRET
};
```

Vamos a crearnos un repo mock (en memoria) para almacenar el perfil del usuario:

_./src/dals/user.model.ts_:

```typescript
export interface User {
  id: number;
  googleId: string;
  displayName: string;
  firstName: string;
  lastName: string;
  image: string;
  email: string;
}
```

_./src/dals/repository/profile.contract.ts_:

```typescript
import { User } from '../user.model.js';

export interface ProfileRepositoryContract {
  userProfileExists: (googleProfileId: string) => Promise<boolean>;
  addNewUser: (user: User) => Promise<User>;
  getUser: (id: number) => Promise<User>;
  getUserByGoogleId: (id: string) => Promise<User>;
}
```

_./src/dals/repository/profile.mock.ts_:

```typescript
import { User } from '../user.model.js';

// Just a dumb memory implementation just use for local dev purpose
// later on migrate to a MongoDb or whatever Db implementation

let lastId: number = 1;
let userCollection: User[] = [];

export const userProfileExists = async (
  googleProfileId: string
): Promise<boolean> => {
  const index =
    userCollection.findIndex(
      (user) => user.googleId === googleProfileId
    );

  return index !== -1;
};

export const addNewUser = async (user: User): Promise<User> => {
  const newUser = {
    ...user,
    id: lastId,
  };

  userCollection = [...userCollection, newUser];

  lastId++;

  return newUser;
};

export const getUser = async (id: number): Promise<User> => {
  const user = userCollection.find((user) => user.id === id);

  return user;
};

export const getUserByGoogleId = async (googleId: string): Promise<User> => {
  const user = userCollection.find((user) => user.googleId === googleId);

  return user;
};
```

Y su barrel

_./src/dals/repository/index.ts_:

```typescript
import * as mockRepository from './profile.mock.js';
import { ProfileRepositoryContract } from './profile.contract.js';
// TODO: add here real repository

// TODO: Check here env variable if we are in mock mode or not
// and choose whether use the mock or the real version
export const profileRepository: ProfileRepositoryContract = mockRepository;
```

Y exponemos con un barrel a nivel de DAL los modulos que necesitemos:

_./src/dals/index.ts_:

```typescript
export * from './user.model.js';
export * from './repository/index.js';
```

Vamos a hacer el setup de _Passport_ y definir la estrategia para trabajar con la autenticación contra Google:

- Recibimos la respuesta de google cuando se ha autenticado con éxito.
- Ahí tenemos la info de la cuenta, profile Id, EMail...
- Lo guardamos en una supuesta base de datos de usuarios (si no existe ya).

_./src/setup/passport-config.ts_:

```ts
import passportGoogle from 'passport-google-oauth20';
import { envConstants } from '../env.constants.js';
import { User, profileRepository } from '../dals/index.js';

const googleStrategy = passportGoogle.Strategy;

export const configPassport = function (passport) {
  passport.use(
    new googleStrategy(
      {
        clientID: envConstants.GOOGLE_CLIENT_ID,
        clientSecret: envConstants.GOOGLE_CLIENT_SECRET,
        callbackURL: '/api/callback',
      },
      async (accessToken, refreshToken, profile, done) => {
        const sessionExists = await profileRepository.userProfileExists(
          profile.id
        );
        if (sessionExists) {
          // Extract user logged in session from repository
          const user = await profileRepository.getUserByGoogleId(profile.id);
          done(null, user);
        } else {
          let user: User = {
            id: -1,
            googleId: profile.id,
            displayName: profile.displayName,
            firstName: profile.name.givenName,
            lastName: profile.name.familyName,
            image: profile.photos[0].value,
            email: profile.emails[0].value,
          };

          // Create new session an store it into the repo
          user = await profileRepository.addNewUser(user);

          done(null, user);
        }
      }
    )
  );
};
```

Añadimos un barrel

_./src/setup/index.ts_:

```ts
export * from './passport-config.js';
```

Es hora de tocar el HTML:

Vamos a añadir un enlace en index para que se vaya a la página de _index.html_

_./public/index.html_:

```html
<!DOCTYPE html>
<html>
  <head>
    <title>Prueba Login</title>
  </head>
  <body>
    <section>
      <a id="login-button" href="/api/google">Login using Google</a>
    </section>
  </body>
</html>
```

Y vamos a crear una página en la que ya asumimos que estamos logados, que hacemos aquí

- Hacemos un fetch
- Mostramos por consola el perfil del usuario que se ha logado (¿Alguien se anima a hacer algo más bonito? Fork :))

_./public/mainapp.html_:

```html
<!DOCTYPE html>
<html>
  <head>
    <title>User already logged in using Google</title>
    <script src="https://polyfill.io/v3/polyfill.min.js?version=3.52.1&features=fetch"></script>
  </head>
  <body>
    <img id="profilepic" />
    <p id="username">user name</p>
    <section>
      <h1>My site: info about user logged in (F12 open console :))</h1>
    </section>
    <script type="text/javascript">
      fetch('/api/user-profile', {
        method: 'GET',
      })
        .then(function (response) {
          return response.json();
        })
        .then(function (session) {
          document.getElementById('profilepic')['src'] = session.image;
          document.getElementById('username')['innerHTML'] =
            session.displayName;

          console.log(session);
        })
        .catch(function (error) {
          console.error('Error:', error);
        });
    </script>
  </body>
</html>
```

- Vamos ahora a por los endpoints de la API (añadir al final del fichero):
  - Añadimos los imports de passport, dal...

_./src/api.ts_:

```diff
import { Router } from 'express';
+ import jwt from 'jsonwebtoken';
+ import passport from 'passport';
+ import { profileRepository, User } from './dals/index.js';

export const api = Router();
```

- Añadimos el endpoint _/google_ que es donde arrancamos el proceso con Passport para que conecte con Google Accounts (redirect a su página).

_./src/api.ts_:

```typescript
// (...)

api.get(
  '/google',
  passport.authenticate('google', {
    scope: ['profile', 'email'],
    session: false, // Default value: true
  })
);
```

- Añadimos un segundo endpoint, el del _callback_, que será el que Google Accounts invocará cuando de la respuesta al login (ojo, que este endpoint lo tenemos que tener como válido en nuestra consola de administración de Google). El identificador del usuario lo almacena en una cookie (httpOnly) en el navegador, cuyo valor será un token JWT.

_./src/api.ts_:

```typescript
// (...)

const JWT_SECRET = 'MY_SECRET'; // TODO: Move to env variable
const COOKIE_NAME = 'authorization';
interface TokenPayload {
  id: number;
}

const createAccessToken = (userId: number) => {
  const payload: TokenPayload = {
    id: userId,
  };
  const token = jwt.sign(payload, JWT_SECRET);
  return token;
};

api.get(
  '/callback',
  passport.authenticate('google', { failureRedirect: '/', session: false }),
  (req, res) => {
    console.log('Ha llegado la respuesta de Google');
    console.log(req.user);  
    const user = req.user as User;
    const token = createAccessToken(user.id);
    res.cookie(COOKIE_NAME, token, {
      httpOnly: true,
      secure: false, // TODO: Enable in production
    });
    res.redirect('/mainapp.html');
  }
);
```

- Añadimos un tercer endpoint que va a ser el que nos de la información del usuario logado cuando la pidamos vía fetch desde nuestra página principal (una vez que ya nos hemos logado). En este endpoint además vamos a:
  - Recuperar el valor de la cookie, es decir, obtener el token JWT y verificar que el token es correcto.
  - Extraer el id del usuario de ese token.
  - Recuperar los datos del perfil de usuario en base a ese id.

_./src/api.ts_:

```typescript
// (...)

const getTokenPayload = async (token: string): Promise<TokenPayload> =>
  new Promise((resolve) => {
    jwt.verify(token, JWT_SECRET, (error, payload) =>
      error ? resolve(null) : resolve(payload as TokenPayload)
    );
  });

api.get('/user-profile', async (req, res) => {
  const token = req.cookies[COOKIE_NAME];
  const payload = await getTokenPayload(token);

  const user = await profileRepository.getUser(payload?.id);

  res.status(user ? 200 : 401).send(user);
});
```

Para finalizar, vamos a hacer uso de la configuración del middleware Passport.js en nuestro punto de entrada (index.ts_).

_./src/index.ts_:

```diff
import express from 'express';
import path from 'path';
import url from 'url';
import { createApp } from './express.server.js';
import { envConstants } from './env.constants.js';
import { api } from './api.js';
+ import cookieParser from 'cookie-parser';
+ import passport from 'passport';
+ import { configPassport } from './setup/index.js';


const app = createApp();
+ app.use(cookieParser());

+ configPassport(passport);
+ // We need to setup the middleware
+ app.use(passport.initialize());

const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
const staticFilesPath = path.resolve(__dirname, envConstants.STATIC_FILES_PATH);
app.use('/', express.static(staticFilesPath));

app.use('/api', api);

app.listen(envConstants.PORT, () => {
  console.log(`Server ready at http://localhost:${envConstants.PORT}/api`);
});
```

- Vamos a ver si esto funciona

```bash
npm start
```

Lo ideal aquí es, después de probar el funcionamiento de nuestro ejemplo, hacer debug en algunos sitios interesantes. Por ejemplo, poner breakpoints en:

_./setup/passport-config.ts_:

Definición y callback

_./api.ts_:

End point `/google`

End point `/callback`

End point `/user-profile`

Para depurar el endpoint `/google` tenemos que añadir esto:

_./src/api.ts_:

```diff
api.get(
  '/google',
+  () =>
    passport.authenticate('google', {
      scope: ['profile', 'email'],
      session: false, // Default value: true
    })
);
```
