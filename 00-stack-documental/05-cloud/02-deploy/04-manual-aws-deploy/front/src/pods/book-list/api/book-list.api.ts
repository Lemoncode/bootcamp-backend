import axios from 'axios';
import { Book } from './book-list.api-model';

const baseUrl = '/api/books';

export const getBookList = async (): Promise<Book[]> => {
  const { data } = await axios.get<Book[]>(baseUrl);

  return data;
};

export const deleteBook = async (id: string): Promise<void> => {
  await axios.delete(`${baseUrl}/${id}`);
};
