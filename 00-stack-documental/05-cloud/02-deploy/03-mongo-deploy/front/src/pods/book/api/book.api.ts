import axios from 'axios';
import { Book } from './book.api-model';

const baseUrl = '/api/books';

export const getBook = async (id: string): Promise<Book> => {
  const { data } = await axios.get<Book>(`${baseUrl}/${id}`);

  return data;
};

export const saveBook = async (book: Book): Promise<Book> => {
  if (book.id) {
    await axios.put(`${baseUrl}/${book.id}`, book);
  } else {
    const { data } = await axios.post<Book>(baseUrl, book);
    return data;
  }
};
