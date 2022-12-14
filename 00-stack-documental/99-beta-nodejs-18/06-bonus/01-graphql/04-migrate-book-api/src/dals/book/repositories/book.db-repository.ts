import { ObjectId } from 'mongodb';
import { bookContext } from '../book.context';
import { BookRepository } from './book.repository';
import { Book } from '../book.model';

export const dbRepository: BookRepository = {
  getBookList: async () => await bookContext.find().lean(),
  getBook: async (id: string) =>
    await bookContext.findOne({ _id: new ObjectId(id) }).lean(),
  saveBook: async (book: Book) =>
    await bookContext
      .findOneAndUpdate(
        {
          _id: book._id,
        },
        { $set: book },
        { upsert: true, new: true }
      )
      .lean(),
  deleteBook: async (id: string) => {
    const { deletedCount } = await bookContext.deleteOne({
      _id: new ObjectId(id),
    });
    return deletedCount === 1;
  },
};
