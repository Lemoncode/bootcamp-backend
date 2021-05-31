import { ObjectId } from "mongodb";
import { getDBInstance } from "core/servers";
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";

export const dbRepository: BookRepository = {
  getBookList: async () => {
    const db = getDBInstance();
    return await db.collection<Book>("books").find().toArray();
  },
  getBook: async (id: string) => {
    const db = getDBInstance();
    return await db.collection<Book>("books").findOne({
      _id: new ObjectId(id),
    });
  },
  saveBook: async (book: Book) => {
    const db = getDBInstance();
    const { value } = await db.collection<Book>("books").findOneAndUpdate(
      {
        _id: book._id,
      },
      { $set: book },
      { upsert: true, returnDocument: "after" }
    );
    return value;
  },
  deleteBook: async (id: string) => {
    const db = getDBInstance();
    const { deletedCount } = await db.collection<Book>("books").deleteOne({
      _id: new ObjectId(id),
    });
    return deletedCount === 1;
  },
};
