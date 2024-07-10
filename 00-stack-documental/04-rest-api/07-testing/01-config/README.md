# 01 Config

In this example we are going to add a basic setup needed to support unit testing with Jest.

We will start from `00-boilerplate`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

# Libraries

We are going to install the main library which we base all our unit tests, [Vitest](https://vitest.dev/).

- [vitest]https://vitest.dev/): JavaScript Testing library with runner, assertion, mocks, etc.
> It includes typings.

```bash
npm install vitest --save-dev
```

# Config

Test commands:
  - `npm test`: to single run
  - `npm run test:watch`: to run all specs after changes.

> NOTE:

> [CLI options](https://vitest.dev/guide/cli.html)

> `run`: single run.

> `watch`: rerun tests when they change.

_./package.json_

```diff
{
  ...
  "scripts": {
    ...
+   "test": "vitest run",
+   "test:watch": "vitest watch"
  },
  ...
}
```

# Dummy spec

Let's launch tests in watch mode:

```bash
npm run test:watch
```

Adding success spec:

_./src/dummy.spec.ts_

```javascript
import { describe, it, expect } from 'vitest';

describe('dummy specs', () => {
  it('should pass spec', () => {
    // Arrange

    // Act

    // Assert
    expect(true).toBeTruthy();
  });
});
```

Adding failed spec:

_./src/dummy.spec.ts_

```diff
...

+ it('should fail spec', () => {
+   // Arrange

+   // Act

+   // Assert
+   expect(true).toBeFalsy();
+ });

});
```

# External config

We could create a vitest config outside `package.json` to improve maintainability.

> [Configuring vitest](https://vitest.dev/guide/#configuring-vitest)
>
> [Configuration options](https://vitest.dev/config)

Create config in `config/test/config.ts` file:

_./config/test/config.ts_

```js
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    globals: true,
  },
});

```
> Enable [globals](https://vitest.dev/config/#globals)
>
> We will add more configuration properties in next examples when we needed

Enable types for globals:

_./config/test/config.d.ts_

```typescript
/// <reference types="vitest/globals" />

```

Update package.json:

_./package.json_

```diff
{
  ...
  "scripts": {
    ...
-   "test": "vitest run",
+   "test": "vitest run -c ./config/test/config.ts",
-   "test:watch": "vitest watch"
+   "test:watch": "vitest watch -c ./config/test/config.ts"
  },
  ...
}
```

Update `tsconfig.json`:

_./tsconfig.json_

```diff{
  "compilerOptions": {
    ...
  },
- "include": ["src/**/*"]
+ "include": ["src/**/*", "config/test/config.d.ts"]
}

```

Running specs again:

```bash
npm run test:watch
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
