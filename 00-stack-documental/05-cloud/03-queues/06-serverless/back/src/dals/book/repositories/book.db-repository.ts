import { ObjectId } from 'mongodb';
import { BookRepository } from './book.repository.js';
import { Book } from '../book.model.js';
import { getBookContext } from '../book.context.js';

export const dbRepository: BookRepository = {
  getBookList: async (page?: number, pageSize?: number) => {
    const skip = Boolean(page) ? (page - 1) * pageSize : 0;
    const limit = pageSize ?? 0;
    return await getBookContext().find().skip(skip).limit(limit).toArray();
  },
  getBook: async (id: string) => {
    return await getBookContext().findOne({
      _id: new ObjectId(id),
    });
  },
  saveBook: async (book: Book) => {
    return await getBookContext().findOneAndUpdate(
      {
        _id: book._id,
      },
      { $set: book },
      { upsert: true, returnDocument: 'after' }
    );
  },
  deleteBook: async (id: string) => {
    const { deletedCount } = await getBookContext().deleteOne({
      _id: new ObjectId(id),
    });
    return deletedCount === 1;
  },
};
