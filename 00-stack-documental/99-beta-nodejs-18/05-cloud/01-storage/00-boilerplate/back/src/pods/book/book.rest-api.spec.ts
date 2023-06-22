import { ObjectId } from 'mongodb';
import supertest from 'supertest';
import {
  createRestApiServer,
  connectToDBServer,
  disconnectFromDBServer,
} from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
import { getBookContext } from '#dals/book/book.context.js';
import { Book } from './book.api-model.js';
import { booksApi } from './book.rest-api.js';

const app = createRestApiServer();
app.use((req, res, next) => {
  req.userSession = {
    id: '1',
    role: 'admin',
  };
  next();
});
app.use(booksApi);

const MONGODB_URI = `${globalThis.__MONGO_URI__}${globalThis.__MONGO_DB_NAME__}`;

describe('pods/book/book.rest-api specs', () => {
  beforeAll(async () => {
    await connectToDBServer(MONGODB_URI);
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
    await disconnectFromDBServer();
  });

  describe('get book list', () => {
    it('should return the whole bookList with values when it request "/" endpoint without query params', async () => {
      // Arrange
      const route = '/';

      // Act
      const response = await supertest(app).get(route);

      // Assert
      expect(response.statusCode).toEqual(200);
      expect(response.body).toHaveLength(1);
    });

    it('should return return 201 when it inserts new book', async () => {
      // Arrange
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
  });
});
