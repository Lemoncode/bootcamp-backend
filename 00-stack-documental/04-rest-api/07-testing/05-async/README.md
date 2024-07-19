# 05 Async

In this example we are going to learn test async code.

We will start from `04-tdd`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install
```

Let's test an important piece of code, the async code. We will start with `authenticationMiddleware`:

Create specs file:

_./src/core/security/security.middlewares.spec.ts_

```typescript
describe('security.middlewares specs', () => {
  describe('authenticationMiddleware', () => {
    it('', () => {
      // Arrange
      // Act
      // Assert
    });
  });
});
```

Should send 401 status code if it feeds authorization cookie equals undefined:

_./src/core/security/security.middlewares.spec.ts_

```diff
+ import { Request, Response } from 'express';
+ import { authenticationMiddleware } from './security.middlewares.js';

describe('core/security/security.middlewares specs', () => {
  describe('authorizationMiddleware', () => {
-   it('', () => {
+   it('should send 401 status code if it feeds authorization cookie equals undefined', () => {
      // Arrange
+     const authorization = undefined;

+     const cookies: Record<string, any> = {
+       authorization,
+     };
+     const req = {
+       cookies,
+     } as Request;
+     const res = {
+       sendStatus: vi.fn() as any,
+       clearCookie: vi.fn() as any,
+     } as Response;
+     const next = vi.fn();

      // Act
+     authenticationMiddleware(req, res, next);

      // Assert
+     expect(res.sendStatus).toHaveBeenCalledWith(401);
+     expect(res.clearCookie).toHaveBeenCalledWith('authorization');
    });
  });
});

```

Run specs:

```bash
npm run test:watch security.middlewares.spec

```

Why is it failing? Because it's an async code and we have to tell `vitest` that it has to wait to resolve `promise`:

_./src/core/security/security.middlewares.spec.ts_

```diff
import { Request, Response } from 'express';
import { authenticationMiddleware } from './security.middlewares.js';

describe('security.middlewares specs', () => {
  describe('authenticationMiddleware', () => {
-   it('should send 401 status code if it feeds authorization cookie equals undefined', () => {
+   it('should send 401 status code if it feeds authorization cookie equals undefined', async () => {
      // Arrange
      const authorization = undefined;

      const cookies: Record<string, any> = {
        authorization,
      };
      const req = {
        cookies,
      } as Request;
      const res = {
        sendStatus: vi.fn() as any,
        clearCookie: vi.fn() as any,
      } as Response;
      const next = vi.fn();

      // Act
+     await vi.waitFor(() => {
        authenticationMiddleware(req, res, next);
+     });

      // Assert
      expect(res.sendStatus).toHaveBeenCalledWith(401);
      expect(res.clearCookie).toHaveBeenCalledWith('authorization');
    });
  });
});


```

Or we update the current type (which is returning a promise):

_./src/core/security/security.middlewares.ts_

```diff
- import { RequestHandler } from 'express';
+ import { RequestHandler, Request, Response, NextFunction } from 'express';
import jwt from 'jsonwebtoken';
import { ENV } from '#core/constants/index.js';
import { UserSession, Role } from '#core/models/index.js';

...

- export const authenticationMiddleware: RequestHandler = async (
+ export const authenticationMiddleware = async (
- req,
+ req: Request,
- res,
+ res: Response,
- next
+ next: NextFunction
) => {
  try {
    const [, token] = req.cookies.authorization?.split(' ') || [];
    const userSession = await verify(token, ENV.AUTH_SECRET);
    req.userSession = userSession;
    next();
  } catch (error) {
    res.sendStatus(401);
  }
};

...
```

And wait the promise itself:

_./src/core/security/security.middlewares.spec.ts_

```diff
...

      // Act
-     await vi.waitFor(() => {
-       authenticationMiddleware(req, res, next);
-     });
+     await authenticationMiddleware(req, res, next);

      // Assert
      expect(res.sendStatus).toHaveBeenCalledWith(401);
    });
  });
});

```

Remember, we are executing the real implementation of `verify` function, if we want to provide different behaviour we need to mock it:

_./src/core/security/security.middlewares.spec.ts_

```diff
import { Request, Response } from 'express';
+ import jwt from 'jsonwebtoken';
+ import { UserSession } from '#core/models/index.js';
import { authenticationMiddleware } from './security.middlewares.js';

...

+   it('should call next function and assign userSession if it feeds authorization cookie with token', async () => {
+     // Arrange
+     const authorization = 'Bearer my-token';
+     const userSession: UserSession = {
+       id: '1',
+       role: 'admin',
+     };
+     vi.spyOn(jwt, 'verify').mockImplementation(
+       (token, secret, callback: any) => {
+         callback(undefined, userSession);
+       }
+     );

+     const cookies: Record<string, any> = {
+       authorization,
+     };
+     const req = {
+       cookies,
+     } as Request;
+     const res = {
+       sendStatus: vi.fn() as any,
+       clearCookie: vi.fn() as any,
+     } as Response;
+     const next = vi.fn();

+     // Act
+     await authenticationMiddleware(req, res, next);

+     // Assert
+     expect(jwt.verify).toHaveBeenCalled();
+   });

...

```

> Add breakpoints

We could simplify specs if we move the `verify` function to a common place:

_./src/common/helpers/jwt.helpers.ts_

```typescript
import jwt from 'jsonwebtoken';

export const verifyJWT = <T>(token: string, secret: string): Promise<T> =>
  new Promise<T>((resolve, reject) => {
    jwt.verify(token, secret, (error, payload) => {
      if (error) {
        reject(error);
      }

      if (payload) {
        resolve(payload as unknown as T);
      } else {
        reject();
      }
    });
  });
```

Update barrel file:

_./src/common/helpers/index.ts_

```diff
export * from './hash.helpers.js';
+ export * from './jwt.helpers.js';

```

Update security middlewares:

_./src/core/security/security.middlewares.ts_

```diff
import { RequestHandler, Request, Response, NextFunction } from 'express';
- import jwt from 'jsonwebtoken';
+ import { verifyJWT } from '#common/helpers/index.js';
import { ENV } from '#core/constants/index.js';
import { UserSession, Role } from '#core/models/index.js';

- const verify = (token: string, secret: string): Promise<UserSession> =>
-   new Promise((resolve, reject) => {
-     jwt.verify(token, secret, (error, userSession: UserSession) => {
-       if (error) {
-         reject(error);
-       }

-       if (userSession) {
-         resolve(userSession);
-       } else {
-         reject();
-       }
-     });
-   });

export const authenticationMiddleware = async (
  req: Request,
  res: Response,
  next: NextFunction
) => {
  try {
    const [, token] = req.cookies.authorization?.split(' ') || [];
-   const userSession = await verify(token, ENV.AUTH_SECRET);
+   const userSession = await verifyJWT<UserSession>(token, ENV.AUTH_SECRET);
    req.userSession = userSession;
    next();
  } catch (error) {
    res.sendStatus(401);
  }
};

...

```

Update specs:

_./src/core/security/security.middlewares.spec.ts_

```diff
import { Request, Response } from 'express';
- import jwt from 'jsonwebtoken';
+ import * as helpers from '#common/helpers/index.js';
import { UserSession } from '#core/models/index.js';
import { authenticationMiddleware } from './security.middlewares.js';

describe('core/security/security.middlewares specs', () => {
  describe('authenticationMiddleware', () => {
    it('should send 401 status code if it feeds authorization cookie equals undefined', async () => {
      // Arrange
      const authorization = undefined;
+     vi.spyOn(helpers, 'verifyJWT').mockRejectedValue('Not valid token');

...
      // Assert
      expect(res.sendStatus).toHaveBeenCalledWith(401);
      expect(res.clearCookie).toHaveBeenCalledWith('authorization');
+     expect(helpers.verifyJWT).toHaveBeenCalled();
    });

    it('should call next function and assign userSession if it feeds authorization cookie with token', async () => {
      // Arrange
      const authorization = 'Bearer my-token';
      const userSession: UserSession = {
        id: '1',
        role: 'admin',
      };
-     vi.spyOn(jwt, 'verify').mockImplementation(
-       (token, secret, callback: any) => {
-         callback(undefined, userSession);
-       }
-     );
+     vi.spyOn(helpers, 'verifyJWT').mockResolvedValue(userSession);

...

      // Assert
-     expect(jwt.verify).toHaveBeenCalled();
+     expect(helpers.verifyJWT).toHaveBeenCalled();
+     expect(next).toHaveBeenCalled();
+     expect(req.userSession).toEqual(userSession);
    });
  });
});

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
