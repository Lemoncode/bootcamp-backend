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

_./src/pods/security/security.middlewares.spec.ts_

```typescript
describe('pods/security/security.middlewares specs', () => {
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

_./src/pods/security/security.middlewares.spec.ts_

```diff
+ import { Request, Response } from 'express';
+ import { authenticationMiddleware } from './security.middlewares';

describe('pods/security/security.middlewares specs', () => {
  describe('authorizationMiddleware', () => {
-   it('', () => {
+   it('should send 401 status code if it feeds authorization cookie equals undefined', () => {
      // Arrange
+     const authorization = undefined;

+     const req = {
+       cookies: {
+         authorization,
+       },
+     } as Request;
+     const res = {
+       sendStatus: jest.fn() as any,
+     } as Response;
+     const next = jest.fn();

      // Act
+     authenticationMiddleware(req, res, next);

      // Assert
+     expect(res.sendStatus).toHaveBeenCalled();
+     expect(res.sendStatus).toHaveBeenCalledWith(401);
    });
  });
});

```

Run specs:

```bash
npm run test:watch security.middlewares.spec

```

Why it cannot find a module `'core/constants'`? Because we need to configure alias in specs too:

_./config/test/jest.js_

```diff
module.exports = {
  rootDir: '../../',
  preset: 'ts-jest',
  restoreMocks: true,
+ moduleDirectories: ['<rootDir>/src', 'node_modules'],
};

```

> [moduleDirectories default value](https://jestjs.io/docs/configuration#moduledirectories-arraystring)
>
> [More info](https://www.basefactor.com/configuring-aliases-in-webpack-vs-code-typescript-jest)

 Why is it still failing? Because it's an async code and we have to tell `jest` that it has to wait to resolve `promise`:

_./src/pods/security/security.middlewares.spec.ts_

```diff
import { Request, Response } from 'express';
import { authenticationMiddleware } from './security.middlewares';

describe('pods/security/security.middlewares specs', () => {
  describe('authenticationMiddleware', () => {
-   it('should send 401 status code if it feeds authorization cookie equals undefined', () => {
+   it('should send 401 status code if it feeds authorization cookie equals undefined', (done) => {
      // Arrange
      const authorization = undefined;

      const req = {
        cookies: {
          authorization,
        },
      } as Request;
      const res = {
        sendStatus: jest.fn() as any,
      } as Response;
      const next = jest.fn();

      // Act
-     authenticationMiddleware(req, res, next);
+     authenticationMiddleware(req, res, next).then(() => {
        // Assert
        expect(res.sendStatus).toHaveBeenCalled();
        expect(res.sendStatus).toHaveBeenCalledWith(401);
+       done();
+     });
    });
  });
});

```

> [Jest testing async code](https://jestjs.io/docs/en/asynchronous.html)

Type implementation response:

_./src/pods/security/security.middlewares.ts_

```diff
- import { RequestHandler } from 'express';
+ import { RequestHandler, Request, Response, NextFunction } from 'express';
import jwt from 'jsonwebtoken';
import { envConstants } from 'core/constants';
import { UserSession, Role } from 'common-app/models';

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
    const userSession = await verify(token, envConstants.AUTH_SECRET);
    req.userSession = userSession;
    next();
  } catch (error) {
    res.sendStatus(401);
  }
};

...
```

A second approach is using `async/await`:

_./src/pods/security/security.middlewares.spec.ts_

```diff
import { Request, Response } from 'express';
import { authenticationMiddleware } from './security.middlewares';

describe('pods/security/security.middlewares specs', () => {
  describe('authenticationMiddleware', () => {
-   it('should send 401 status code if it feeds authorization cookie equals undefined', (done) => {
+   it('should send 401 status code if it feeds authorization cookie equals undefined', async () => {
      // Arrange
      const authorization = undefined;

      const req = {
        cookies: {
          authorization,
        },
      } as Request;
      const res = {
        sendStatus: jest.fn() as any,
      } as Response;
      const next = jest.fn();

      // Act
-     authenticationMiddleware(req, res, next).then(() => {
+     await authenticationMiddleware(req, res, next);

        // Assert
        expect(res.sendStatus).toHaveBeenCalled();
        expect(res.sendStatus).toHaveBeenCalledWith(401);
-       done();
-     });
    });
  });
});

```

Remember, we are executing the real implementation of `verify` function, if we want to provide different behaviour we need to mock it:

_./src/pods/security/security.middlewares.spec.ts_

```diff
import { Request, Response } from 'express';
+ import jwt from 'jsonwebtoken';
+ import { UserSession } from 'common-app/models';
import { authenticationMiddleware } from './security.middlewares';

...

+   it('should call next function and assign userSession if it feeds authorization cookie with token', async () => {
+     // Arrange
+     const authorization = 'Bearer my-token';
+     const userSession: UserSession = {
+       id: '1',
+       role: 'admin',
+     };
+     const verifyStub = jest
+       .spyOn(jwt, 'verify')
+       .mockImplementation((token, secret, callback: any) => {
+         callback(undefined, userSession);
+       });

+     const req = {
+       cookies: {
+         authorization,
+       },
+     } as Request;
+     const res = {
+       sendStatus: jest.fn() as any,
+     } as Response;
+     const next = jest.fn();

+     // Act
+     await authenticationMiddleware(req, res, next);

+     // Assert
+     expect(verifyStub).toHaveBeenCalled();
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
export * from './hash-password.helpers';
+ export * from './jwt.helpers';

```

Update security middlewares:

_./src/pods/security/security.middlewares.ts_

```diff
import { RequestHandler, Request, Response, NextFunction } from 'express';
- import jwt from 'jsonwebtoken';
import { envConstants } from 'core/constants';
+ import { verifyJWT } from 'common/helpers';
import { UserSession, Role } from 'common-app/models';

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
-   const userSession = await verify(token, envConstants.AUTH_SECRET);
+   const userSession = await verifyJWT<UserSession>(token, envConstants.AUTH_SECRET);
    req.userSession = userSession;
    next();
  } catch (error) {
    res.sendStatus(401);
  }
};

...

```

Update specs:

_./src/pods/security/security.middlewares.spec.ts_

```diff
import { Request, Response } from 'express';
- import jwt from 'jsonwebtoken';
+ import * as helpers from 'common/helpers/jwt.helpers';
import { UserSession } from 'common-app/models';
import { authenticationMiddleware } from './security.middlewares';

describe('pods/security/security.middlewares specs', () => {
  describe('authenticationMiddleware', () => {
    it('should send 401 status code if it feeds authorization cookie equals undefined', async () => {
      // Arrange
      const authorization = undefined;
+     const verifyJWTStub = jest
+       .spyOn(helpers, 'verifyJWT')
+       .mockRejectedValue('Not valid token');

...
      // Assert
      expect(res.sendStatus).toHaveBeenCalled();
      expect(res.sendStatus).toHaveBeenCalledWith(401);
+     expect(verifyJWTStub).toHaveBeenCalled();
    });

    it('should call next function and assign userSession if it feeds authorization cookie with token', async () => {
      // Arrange
      const authorization = 'Bearer my-token';
      const userSession: UserSession = {
        id: '1',
        role: 'admin',
      };
-     const verifyStub = jest
-       .spyOn(jwt, 'verify')
-       .mockImplementation((token, secret, callback: any) => {
-         callback(undefined, userSession);
-       });
+     const verifyJWTStub = jest
+       .spyOn(helpers, 'verifyJWT')
+       .mockResolvedValue(userSession);

...

      // Assert
-     expect(verifyStub).toHaveBeenCalled();
+     expect(helpers.verifyJWT).toHaveBeenCalled();
+     expect(next).toHaveBeenCalled();
+     expect(req.userSession).toEqual(userSession);
    });
  });
});

```

> NOTE: don't use barrels in spyOn methods.
>
> [Related issue](https://github.com/facebook/jest/issues/6914)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
