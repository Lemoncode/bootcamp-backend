import * as apiModel from './api';
import * as viewModel from './book.vm';

export const mapBookFromApiToVM = (book: apiModel.Book): viewModel.Book => ({
  id: book.id,
  title: book.title,
  releaseDate: new Date(book.releaseDate).toLocaleDateString(undefined, {
    month: '2-digit',
    day: '2-digit',
    year: 'numeric',
  }),
  author: book.author,
});

export const mapBookFromVMToApi = (book: viewModel.Book): apiModel.Book => {
  const [day, month, year] = book.releaseDate.split('/');
  return {
    id: book.id,
    title: book.title,
    releaseDate: new Date(`${year}-${month}-${day}`).toISOString(),
    author: book.author,
  };
};
