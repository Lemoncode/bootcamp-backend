import { Book } from "../book.model.js";

export interface BookRepository {
  getBookList: () => Promise<Book[]>;
  getBook: (id: string) => Promise<Book>;
  saveBook: (book: Book) => Promise<Book>;
  deleteBook: (id: string) => Promise<boolean>;
}
