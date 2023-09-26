# 04 TDD

In this example we are going to apply Test Driven Development while we are implementing the app.

We will start from `03-debug`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install
```

Let's remove all `calculator` stuff.
  - `./src/business/calculator.business.ts`
  - `./src/business/index.ts`
  - `./src/calculator.spec.ts`
  - `./src/calculator.ts`
  - `./src/second.spec.ts`

Run tests in watch mode:

```bash
npm run test:watch
```

Let's add a new mapper function to `books.mappers`, but this time, we will start from `spec`:

_./src/pods/book/book.mappers.spec.ts_

```typescript
describe('book.mappers spec', () => {
  describe('mapBookListFromApiToModel', () => {
    it('', () => {
      // Arrange
      // Act
      // Assert
    });
  });
});

```

Should return empty array when it feeds undefined:

_./src/pods/book/book.mappers.spec.ts_

```diff
+ import * as model from '#dals/index.js';
+ import * as apiModel from './book.api-model.js';
+ import {} from './book.mappers.js';

describe('pods/book/book.mappers spec', () => {
  describe('mapBookListFromApiToModel', () => {
-   it('', () => {
+   it('should return empty array when it feeds bookList equals undefined', () => {
      // Arrange
+     const bookList: apiModel.Book[] = undefined;

      // Act
+     const result = mapBookListFromApiToModel(bookList);

      // Assert
+     const expectedResult: model.Book[] = [];
+     expect(result).toEqual(expectedResult);
    });
  });
});

```

Create the minimum implementation to pass the test:

_./src/pods/book/book.mappers.ts_

```diff
...

+ export const mapBookListFromApiToModel = (
+   bookList: apiModel.Book[]
+ ): model.Book[] => [];

```

Let's update the spec:

_./src/pods/book/book.mappers.spec.ts_

```diff
import * as model from '#dals/index.js';
import * as apiModel from './book.api-model.js';
- import {} from './book.mappers.js';
+ import { mapBookListFromApiToModel } from './book.mappers.js';

...

```

Should return empty array when it feeds null:

_./src/pods/book/book.mappers.spec.ts_

```diff
...

+   it('should return empty array when it feeds bookList equals null', () => {
+     // Arrange
+     const bookList: apiModel.Book[] = null;

+     // Act
+     const result: model.Book[] = mapBookListFromApiToModel(bookList);

+     // Assert
+     expect(result).toEqual([]);
+   });
  });
});

```

Should return empty array when it feeds empty array:

_./src/pods/book/book.mappers.spec.ts_

```diff
...

+   it('should return empty array when it feeds bookList equals empty array', () => {
+     // Arrange
+     const bookList: apiModel.Book[] = [];

+     // Act
+     const result: model.Book[] = mapBookListFromApiToModel(bookList);

+     // Assert
+     expect(result).toEqual([]);
+   });
  });
});

```

Yep, we are ready to deploy to production :^). Should return array one mapped item when it feed array with one item:

_./src/pods/book/book.mappers.spec.ts_

```diff
+ import { ObjectId } from 'mongodb';
import * as model from '#dals/index.js';
import * as apiModel from './book.api-model.js';
import { mapBookListFromApiToModel } from './book.mappers.js';

...

+   it('should return one mapped item in array when it feeds bookList with one item', () => {
+     // Arrange
+     const bookList: apiModel.Book[] = [
+       {
+         id: '60c20a334bec6a37b08acec9',
+         title: 'test-title',
+         releaseDate: '2021-07-28T12:30:00',
+         author: 'test-author',
+       },
+     ];

+     // Act
+     const result: model.Book[] = mapBookListFromApiToModel(bookList);

+     // Assert
+     expect(result).toEqual([
+       {
+         _id: new ObjectId('60c20a334bec6a37b08acec9'),
+         title: 'test-title',
+         releaseDate: new Date('2021-07-28T12:30:00'),
+         author: 'test-author',
+       },
+     ]);
+   });
  });
});

```

Let's update the implementation:

_./src/pods/book/book.mappers.ts_

```diff
...

export const mapBookListFromApiToModel = (
  bookList: apiModel.Book[]
- ): model.Book[] => [];
+ ): model.Book[] => bookList.map(mapBookFromApiToModel);

```

We've break two specs! How to solve them?. Let's start with undefined:

_./src/pods/book/book.mappers.ts_

```diff
...

export const mapBookListFromApiToModel = (
  bookList: apiModel.Book[]
- ): model.Book[] => bookList.map(mapBookFromApiToModel);
+ ): model.Book[] =>
+   bookList !== undefined ? bookList.map(mapBookFromApiToModel) : [];

```

Let's continue with null:

_./src/pods/book/book.mappers.ts_

```diff
...

export const mapBookListFromApiToModel = (
  bookList: apiModel.Book[]
): model.Book[] =>
- bookList !== undefined ? bookList.map(mapBookFromApiToModel) : [];
+ bookList !== undefined && bookList !== null
    ? bookList.map(mapBookFromApiToModel)
    : [];

```

Or if we know about JavaScript Array [isArray](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/isArray) method:

_./src/pods/book/book.mappers.ts_

```diff
...

export const mapBookListFromApiToModel = (
  bookList: apiModel.Book[]
): model.Book[] =>
- bookList !== undefined && bookList !== undefined
+ Array.isArray(bookList)
    ? bookList.map(mapBookFromApiToModel)
    : [];

```

- Another tool provided by jest is the [each](https://jestjs.io/docs/api#testeachtablename-fn-timeout) method.

> We could have some issues typing arrays.
> That's why the `any` casting

### ./src/mapper.spec.ts

```diff
...

describe('mapper specs', () => {
  describe('mapBookListFromApiToModel', () => {
+   it.each<apiModel.Book[]>([undefined, null, []])(
+     'should return empty array when it feeds bookList equals %p',
+     (bookList: any) => {
+       // Arrange

+       // Act
+       const result: model.Book[] = mapBookListFromApiToModel(bookList);

+       // Assert
+       expect(result).toEqual([]);
+     }
+   );

-   it('should return empty array when it feeds bookList equals undefined', () => {
-     // Arrange
-     const bookList: apiModel.Book[] = undefined;

-     // Act
-     const result: model.Book[] = mapBookListFromApiToModel(bookList);

-     // Assert
-     expect(result).toEqual([]);
-   });

-   it('should return empty array when it feeds bookList equals null', () => {
-     // Arrange
-     const bookList: apiModel.Book[] = null;

-     // Act
-     const result: model.Book[] = mapBookListFromApiToModel(bookList);

-     // Assert
-     expect(result).toEqual([]);
-   });

-   it('should return empty array when it feeds bookList equals empty array', () => {
-     // Arrange
-     const bookList: apiModel.Book[] = [];

-     // Act
-     const result: model.Book[] = mapBookListFromApiToModel(bookList);

-     // Assert
-     expect(result).toEqual([]);
-   });

...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
