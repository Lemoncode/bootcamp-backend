import { ObjectId } from 'mongodb';
import * as model from 'dals';
import * as apiModel from './book.api-model';

export const mapBookFromModelToApi = (book: model.Book): apiModel.Book => ({
  id: book._id.toHexString(),
  title: book.title,
  releaseDate: book.releaseDate?.toISOString(),
  author: book.author,
});

export const mapBookListFromModelToApi = (
  bookList: model.Book[]
): apiModel.Book[] => bookList.map(mapBookFromModelToApi);

export const mapBookFromApiToModel = (book: apiModel.Book): model.Book => ({
  _id: new ObjectId(book.id),
  title: book.title,
  releaseDate: new Date(book.releaseDate),
  author: book.author,
});

export const mapBookListFromApiToModel = (
  bookList: apiModel.Book[]
): model.Book[] =>
  Array.isArray(bookList) ? bookList.map(mapBookFromApiToModel) : [];
