import { ObjectId } from 'mongodb';
import supertest from 'supertest';
import { createRestApiServer, dbServer } from '#core/servers/index.js';
import { ENV } from '#core/constants/index.js';
import { getBookContext } from '#dals/book/book.context.js';
import { Book } from './book.api-model.js';
import { bookApi } from './book.api.js';

describe('pods/book/book.api specs', () => {
  beforeAll(async () => {
    await dbServer.connect(ENV.MONGODB_URL);
  });

  beforeEach(async () => {
    await getBookContext().insertOne({
      _id: new ObjectId(),
      title: 'book-1',
      author: 'author-1',
      releaseDate: new Date('2021-07-28'),
    });
  });

  afterEach(async () => {
    await getBookContext().deleteMany({});
  });

  afterAll(async () => {
    await dbServer.disconnect();
  });

  describe('get book list', () => {
    const app = createRestApiServer();

    beforeAll(() => {
      app.use(bookApi);
    });

    it('should return the whole bookList with values when it request "/" endpoint without query params', async () => {
      // Arrange
      const route = '/';

      // Act
      const response = await supertest(app).get(route);

      // Assert
      expect(response.statusCode).toEqual(200);
      expect(response.body).toHaveLength(1);
    });
  });

  describe('insert book', () => {
    it('should return 201 when an admin user inserts new book', async () => {
      // Arrange
      const app = createRestApiServer();
      app.use((req, res, next) => {
        req.userSession = {
          id: '1',
          role: 'admin',
        };
        next();
      });
      app.use(bookApi);

      const route = '/';
      const newBook: Book = {
        id: undefined,
        title: 'book-2',
        author: 'author-2',
        releaseDate: '2021-07-29T00:00:00.000Z',
      };

      // Act
      const response = await supertest(app).post(route).send(newBook);

      // Assert
      expect(response.statusCode).toEqual(201);
      expect(response.body.id).toEqual(expect.any(String));
      expect(response.body.title).toEqual(newBook.title);
      expect(response.body.author).toEqual(newBook.author);
      expect(response.body.releaseDate).toEqual(newBook.releaseDate);
    });

    it('should return 403 when a standard user try to insert new book', async () => {
      // Arrange
      const app = createRestApiServer();
      app.use((req, res, next) => {
        req.userSession = {
          id: '1',
          role: 'standard-user',
        };
        next();
      });
      app.use(bookApi);

      const route = '/';
      const newBook: Book = {
        id: undefined,
        title: 'book-2',
        author: 'author-2',
        releaseDate: '2021-07-29T00:00:00.000Z',
      };

      // Act
      const response = await supertest(app).post(route).send(newBook);

      // Assert
      expect(response.statusCode).toEqual(403);
    });
  });
});
