# 02 Login cookies

In this example we are going to implement a login workflow using cookies.

We will start from `01-login-headers`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Another approach is get the current token in a cookie instead of header, we will need install [cookie-parser](https://github.com/expressjs/cookie-parser#readme) library to works with that:

```bash
npm install cookie-parser --save
npm install @types/cookie-parser --save-dev
```

Configure it:

_./src/core/servers/rest-api.server.ts_

```diff
import express from 'express';
import cors from 'cors';
+ import cookieParser from 'cookie-parser';
import { ENV } from "../constants/index.js";

export const createRestApiServer = () => {
  const app = express();
  app.use(express.json());
  app.use(
    cors({
      methods: ENV.CORS_METHODS,
      origin: ENV.CORS_ORIGIN,
    })
  );
+ app.use(cookieParser());

  return app;
};

```

Let's update `login` method:

_./src/pods/security/security.api.ts_

```diff
...
        const token = jwt.sign(userSession, ENV.AUTH_SECRET, {
          expiresIn: '1d',
          algorithm: 'HS256',
        });
-       res.send(`Bearer ${token}`);
+       // TODO: Move to constants
+       res.cookie('authorization', `Bearer ${token}`, {
+         httpOnly: true,
+         secure: ENV.IS_PRODUCTION,
+       });
+       res.sendStatus(204);
      } else {
        res.sendStatus(401);
      }
    } catch (error) {
      next(error);
    }
  })
...

```

> [Cookie options](https://github.com/pillarjs/cookies#cookiesset-name--value---options--)

Now, we will update the `authentication` middleware:

_./src/core/security/security.middlewares.ts_

```diff
...

export const authenticationMiddleware: RequestHandler = async (
  req,
  res,
  next
) => {
  try {
-   const [, token] = req.headers.authorization?.split(' ') || [];
+   const [, token] = req.cookies.authorization?.split(' ') || [];
    const userSession = await verify(token, ENV.AUTH_SECRET);
    req.userSession = userSession;
    next();
  } catch (error) {
    res.sendStatus(401);
  }
};
...
```

Let's run app:

```bash
npm start

```

```md
URL: http://localhost:3000/api/security/login
METHOD: POST

BODY:
{
	"email": "admin@email.com",
	"password": "test"
}

URL: http://localhost:3000/api/books
METHOD: GET

```

> With cookies we could check it on browser too.

Update `logout` method:

_./src/pods/security/security.api.ts_

```diff
...
  .post('/logout', async (req, res) => {
    // NOTE: We cannot invalidate token using jwt libraries.
    // Different approaches:
    // - Short expiration times in token
    // - Black list tokens on DB
+   res.clearCookie('authorization');
    res.sendStatus(200);
  });

```

```md
URL: http://localhost:3000/api/security/logout
METHOD: POST

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
