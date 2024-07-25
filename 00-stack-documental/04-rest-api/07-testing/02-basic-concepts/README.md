# 02 Basic concepts

In this example we are going to add a basic example to test plain vanilla javascript.

We will start from `01-config`.

Summary steps:

- Create `calculator` business.
- Add unit tests.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install
```

Create calculator:

_./src/calculator.ts_

```javascript
export const add = (a, b) => a + b;
```

Rename `dummy.spec.ts` to `calculator.spec.ts`:

_./src/calculator.spec.ts_

```diff
+ import * as calculator from "./calculator.js";

- describe('dummy specs', () => {
+ describe("Calculator specs", () => {
+   describe("add", () => {
-     it('should pass spec', () => {
+     it("should return 4 when passing A equals 2 and B equals 2", () => {
        // Arrange
+       const a = 2;
+       const b = 2;

        // Act
+       const result = calculator.add(a, b);

        // Assert
-       expect(true).toBeTruthy();
+       expect(result).toEqual(4);
      });

-     it('should fail spec', () => {
-       // Arrange

-       // Act

-       // Assert
-       expect(true).toBeFalsy();
-     });
+  });
});

```

> Vitest is ready to support native ESM

Let's check differences between `toEqual` vs `toBe` vs `toStrictEqual`:

_./src/calculator.spec.ts_

```diff
import * as calculator from './calculator.js';

describe('Calculator tests', () => {
  describe('add', () => {
    it('should return 4 when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(result).toEqual(4);
+     expect({ id: 1 }).toBe({ id: 1 });
+     expect({ id: 1 }).toStrictEqual({ id: 1, name: undefined });
    });
  });
});

```

> `toBe` fails if `expect({ id: 1 }).toBe({ id: 1 });`: it's not the same object. We should use `toEqual` if we only want the value not the reference
>
> `toStrictEqual` pass if `expect({ id: 1 }).toStrictEqual({ id: 1 });` but it fails if `expect({ id: 1 }).toStrictEqual({ id: 1, name: undefined });`: it should have same fields, even undefined values. We should use `toEqual` if we don't care about it.

Now, we need passing a method as parameter, whatever it is, we only want to check that it was called and with which arguments:

_./src/calculator.ts_

```diff
- export const add = (a, b) => a + b
+ export const add = (a, b, isLowerThanFive) => {
+   const result = a + b;

+   if (result < 5) {
+     isLowerThanFive(result);
+   }

+   return result;
+ }

```

How we could test it? Using a `spy`:

_./src/calculator.spec.ts_

```diff
import * as calculator from "./calculator.js";

describe("Calculator tests", () => {
  describe("add", () => {
    it("should return 4 when passing A equals 2 and B equals 2", () => {
      // Arrange
      const a = 2;
      const b = 2;
+     const isLowerThanFive = () => {};

      // Act
-     const result = calculator.add(a, b);
+     const result = calculator.add(a, b, isLowerThanFive);

      // Assert
      expect(result).toEqual(4);
    });

+   it('should call to isLowerThanFive when passing A equals 2 and B equals 2', () => {
+     // Arrange
+     const a = 2;
+     const b = 2;
+     const isLowerThanFive = vi.fn();

+     // Act
+     const result = calculator.add(a, b, isLowerThanFive);

+     // Assert
+     expect(isLowerThanFive).toHaveBeenCalled();
+     expect(isLowerThanFive).toHaveBeenCalledWith(4);
+   });
  });
});

```

> If we set `a = 3` this test fail.

Sometimes, we need to `import` dependencies that we can't pass throught function parameters, we need to import as `external dependency`:

_./src/business/calculator.business.ts_

```javascript
export const isLowerThanFive = (value) => {
  console.log(`The value: ${value} is lower than 5`);
};
```

- Add barrel file:

_./src/business/index.ts_

```javascript
export * from './calculator.business.js';
```

Use it:

_./src/calculator.ts_

```diff
+ import { isLowerThanFive } from './business/index.js';

- export const add = (a, b, isLowerThanFive) => {
+ export const add = (a, b) => {
  const result = a + b;

  if(result < 5) {
    isLowerThanFive(result);
  }

  return result;
}

```

Same as before, we only want to test that function was called and with which arguments, but this time is an `external dependency`, so we need a stub:

_./src/calculator.spec.ts_

```diff
import * as calculator from './calculator.js';
+ import * as business from './business/index.js';

describe('Calculator tests', () => {
  describe('add', () => {
    it('should return 4 when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
-     const isLowerThanFive = () => {};

      // Act
-     const result = calculator.add(a, b, isLowerThanFive);
+     const result = calculator.add(a, b);

      // Assert
      expect(result).toEqual(4)
    })

    it('should call to isLowerThanFive when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
-     const isLowerThanFive = vi.fn();
+     const isLowerThanFive = vi.spyOn(business, 'isLowerThanFive');

      // Act
-     const result = calculator.add(a, b, isLowerThanFive);
+     const result = calculator.add(a, b);

      // Assert
      expect(isLowerThanFive).toHaveBeenCalled();
      expect(isLowerThanFive).toHaveBeenCalledWith(4);
    })
  })
})

```

Mocking original behaviour:

_./src/calculator.spec.ts_

```diff
...

    it('should call to isLowerThanFive when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
-     const isLowerThanFive = vi.spyOn(business, 'isLowerThanFive');
+     const isLowerThanFive = vi.spyOn(business, 'isLowerThanFive')
+       .mockImplementation((result) => console.log(`This is the result ${result}`));

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(isLowerThanFive).toHaveBeenCalled();
      expect(isLowerThanFive).toHaveBeenCalledWith(4);
    })

```

Note, it's important reset the `mocks` implementation:

_./src/calculator.spec.ts_

```diff
...

+   it('should call to original implementation isLowerThanFive', () => {
+     // Arrange
+     const a = 1;
+     const b = 2;

+     // Act
+     const result = calculator.add(a, b);

+     // Assert
+     expect(result).toEqual(3);
+   });

```

> console.log
> This is the result 3

We should restore all mocks after run them:

_./src/calculator.spec.ts_

```diff
...

describe('Calculator tests', () => {
+ afterEach(() => {
+   vi.restoreAllMocks();
+ });

  describe('add', () => {
...

```

Instead of use `restoreAllMocks` on each spec file, we could configure it globally:

_./config/test/config.ts_

```diff
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    globals: true,
+   restoreMocks: true,
  },
});

```

> [restoreMocks](https://vitest.dev/config/#restoremocks)

_./src/calculator.spec.ts_

```diff
...

describe('Calculator tests', () => {
- afterEach(() => {
-   vi.restoreAllMocks();
- });

  describe('add', () => {
...

```

> Run again `npm run test:watch`

Finally, we could have a business with too much methods, or even, it is exporting an object:

_./src/business/calculator.business.ts_

```diff
- export const isLowerThanFive = (value) => {
+ export const isLowerThan = (value, max) => {
- console.log(`The value: ${value} is lower than 5`)
+ console.log(`The value: ${value} is lower than ${max}`);
}

+ export const max: number = 6;

```

Use it:

_./src/calculator.ts_

```diff
- import { isLowerThanFive } from './business/index.js';
+ import { isLowerThan, max } from './business/index.js';

export const add = (a, b) => {
  const result = a + b;

- if(result < 5) {
+ if(result < max) {
-   isLowerThanFive(result);
+   isLowerThan(result, max);
  }

  return result;
}

```

In this case, we need to mock the whole module:

_./src/calculator.spec.ts_

```diff
...
describe('Calculator tests', () => {
  describe('add', () => {
...

-   it('should call to isLowerThanFive when passing A equals 2 and B equals 2', () => {
+   it('should call to isLowerThan when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
-     const isLowerThanFive = vi.spyOn(business, 'isLowerThanFive')
-       .mockImplementation((result) => console.log(`This is the result ${result}`));
+     vi.spyOn(business, 'isLowerThan').mockImplementation((result) =>
+       console.log(`This is the result ${result}`)
+     );
+     vi.spyOn(business, 'max', 'get').mockReturnValue(7);

      // Act
      const result = calculator.add(a, b);

      // Assert
-     expect(isLowerThanFive).toHaveBeenCalled();
+     expect(business.isLowerThan).toHaveBeenCalled();
-     expect(isLowerThanFive).toHaveBeenCalledWith(4);
+     expect(business.isLowerThan).toHaveBeenCalledWith(4, 7);
    })

-   it('should call to original implementation isLowerThanFive', () => {
+   it('should call to original implementation isLowerThan', () => {
      // Arrange
      const a = 1;
      const b = 2;

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(result).toEqual(3);
    });
  })
})

```

> If we change max value to 3. spec fails.

When to use `mock` instead of `spyOn` (stub)? If we need to replace original function behaviour or something special that we cannot reach with `spyOn`, for example if a third party library does not support ES Modules like [jsonwebtoken](https://github.com/auth0/node-jsonwebtoken):

_./src/calculator.ts_

```diff
+ import { sign } from 'jsonwebtoken';
import { isLowerThan, max } from './business/index.js';

export const add = (a, b) => {
  const result = a + b;

  if (result < max) {
    isLowerThan(result, max);
+   const token = sign(result, 'my-secret');
+   console.log({ token });
  }

  return result;
};

```

_./src/calculator.spec.ts_

```diff
+ import * as jwt from 'jsonwebtoken';
import * as calculator from './calculator.js';
import * as business from './business/index.js';

...

    it('should call to isLowerThan when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
      vi.spyOn(business, 'isLowerThan').mockImplementation((result) =>
        console.log(`This is the result ${result}`)
      );
      vi.spyOn(business, 'max', 'get').mockReturnValue(7);
+     vi.spyOn(jwt, 'sign').mockImplementation((result) => {
+       console.log(`Sign result ${result}`);
+       return '';
+     });

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(business.isLowerThan).toHaveBeenCalled();
      expect(business.isLowerThan).toHaveBeenCalledWith(4, 7);
+     expect(jwt.sign).toHaveBeenCalledWith(4, 'my-secret');
    });
...

```

It throws the `TypeError: Cannot redefine property: sign` error. It means that we cannot replace the original function behaviour since it isn't a `ESM` module. We should update the code:

_./src/calculator.spec.ts_

```diff
import * as jwt from 'jsonwebtoken';
import * as calculator from './calculator.js';
import * as business from './business/index.js';

+ vi.mock('jsonwebtoken', async (importOriginal) => {
+   const original: any = await importOriginal();
+   return {
+     ...original.default,
+     __esModule: true,
+   };
+ });

...

```

> [Related issue](https://github.com/vitest-dev/vitest/issues/3680)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
