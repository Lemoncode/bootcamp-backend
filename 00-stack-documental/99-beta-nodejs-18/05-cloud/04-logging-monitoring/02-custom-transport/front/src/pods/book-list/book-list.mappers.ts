import * as apiModel from './api';
import * as viewModel from './book-list.vm';

const mapBookFromApiToVM = (book: apiModel.Book): viewModel.Book => ({
  id: book.id,
  title: book.title,
  releaseDate: new Date(book.releaseDate).toLocaleDateString(undefined, {
    month: '2-digit',
    day: '2-digit',
    year: 'numeric',
  }),
  author: book.author,
});

export const mapBookListFromApiToVM = (
  bookList: apiModel.Book[]
): viewModel.Book[] =>
  Array.isArray(bookList) ? bookList.map(mapBookFromApiToVM) : [];
