import { ObjectId } from "mongodb";
import { BookContext } from "../book.context";
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";

export const dbRepository: BookRepository = {
  getBookList: async () => await BookContext.find().lean(),
  getBook: async (id: string) =>
    await BookContext.findOne({ _id: new ObjectId(id) }).lean(),
  saveBook: async (book: Book) =>
    await BookContext.findOneAndUpdate(
      {
        _id: book._id,
      },
      { $set: book },
      { upsert: true, new: true }
    ).lean(),
  deleteBook: async (id: string) => {
    const { deletedCount } = await BookContext.deleteOne({
      _id: new ObjectId(id),
    });
    return deletedCount === 1;
  },
};
