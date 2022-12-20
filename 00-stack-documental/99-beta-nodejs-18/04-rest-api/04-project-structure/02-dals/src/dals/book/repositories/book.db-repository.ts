import { BookRepository } from "./book.repository.js";
import { Book } from "../book.model.js";

export const dbRepository: BookRepository = {
  getBookList: async () => {
    throw new Error("Not implemented");
  },
  getBook: async (id: string) => {
    throw new Error("Not implemented");
  },
  saveBook: async (book: Book) => {
    throw new Error("Not implemented");
  },
  deleteBook: async (id: string) => {
    throw new Error("Not implemented");
  },
};
