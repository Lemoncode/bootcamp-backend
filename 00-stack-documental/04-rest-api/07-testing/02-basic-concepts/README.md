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
+ import * as calculator from "./calculator";

- describe('dummy specs', () => {
+ describe("Calculator tests", () => {
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
import * as calculator from "./calculator";

describe("Calculator tests", () => {
  describe("add", () => {
    it("should return 4 when passing A equals 2 and B equals 2", () => {
      // Arrange
      const a = 2;
      const b = 2;
+     const isLowerThanFive = jest.fn();

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
+     const isLowerThanFive = jest.fn();

+     // Act
+     const result = calculator.add(a, b, isLowerThanFive);

+     // Assert
+     expect(isLowerThanFive).toHaveBeenCalled();
+     expect(isLowerThanFive).toHaveBeenCalledWith(4);
+   })
  });
});

```

> If we set `a = 3` this test fail.

Sometimes, we need to `import` dependencies that we can't pass throught function parameters, we need to import as `external dependency`:

_./src/business.ts_

```javascript
export const isLowerThanFive = value => {
  console.log(`The value: ${value} is lower than 5`);
};
```

Use it:

_./src/calculator.ts_

```diff
+ import { isLowerThanFive } from './business';

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
import * as calculator from './calculator';
+ import * as business from './business';

describe('Calculator tests', () => {
  describe('add', () => {
    it('should return 4 when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
-     const isLowerThanFive = jest.fn();

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
-     const isLowerThanFive = jest.fn();
+     const isLowerThanFive = jest.spyOn(business, 'isLowerThanFive');

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

> Note: As we see in `console`, the `stub` doesn't replace original function behaviour. We have to mock it if we need it.

Mocking original behaviour:

_./src/calculator.spec.ts_

```diff
...

    it('should call to isLowerThanFive when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
-     const isLowerThanFive = jest.spyOn(business, 'isLowerThanFive');
+     const isLowerThanFive = jest.spyOn(business, 'isLowerThanFive')
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
>    This is the result 3

We should restore all mocks after run them:

_./src/calculator.spec.ts_

```diff
...

describe('Calculator tests', () => {
+ afterEach(() => {
+   jest.restoreAllMocks();
+ });

  describe('add', () => {
...

```

Instead of use `restoreAllMocks` on each spec file, we could configure it globally:

_./config/test/jest.js_

```diff
module.exports = {
  rootDir: '../../',
  preset: 'ts-jest',
+ restoreMocks: true,
};

```
> [Jest configuration options](https://facebook.github.io/jest/docs/en/configuration.html#options)

_./src/calculator.spec.ts_

```diff
...

describe('Calculator tests', () => {
- afterEach(() => {
-   jest.restoreAllMocks();
- });

  describe('add', () => {
...

```

> Run again `npm run test:watch`

Finally, we could have a business with too much methods, or even, it is exporting an object:

_./src/business.ts_

```diff
- export const isLowerThanFive = (value) => {
+ export const isLowerThan = (value, max) => {
- console.log(`The value: ${value} is lower than 5`)
+ console.log(`The value: ${value} is lower than ${max}`)
}

+ export const max = 6

```

Use it:

_./src/calculator.ts_

```diff
- import { isLowerThanFive } from './business'
+ import { isLowerThan, max } from './business'

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
import * as calculator from './calculator'
import * as business from './business'

+ jest.mock('./business', () => ({
+   isLowerThan: jest.fn().mockImplementation(() => {
+     console.log('Another implementation');
+   }),
+   max: 7,
+ }));

describe('Calculator tests', () => {
  describe('add', () => {
    it('should return 4 when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(result).toEqual(4)
    })

-   it('should call to isLowerThanFive when passing A equals 2 and B equals 2', () => {
+   it('should call to isLowerThan when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
-     const isLowerThanFive = jest.spyOn(business, 'isLowerThanFive')
-       .mockImplementation((result) => console.log(`This is the result ${result}`));

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

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
