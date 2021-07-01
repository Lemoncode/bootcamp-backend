import { Book } from 'dals';

export const paginateBookList = (
  bookList: Book[],
  page: number,
  pageSize: number
): Book[] => {
  let paginatedBookList = [...bookList];
  if (page && pageSize) {
    const startIndex = (page - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, paginatedBookList.length);
    paginatedBookList = paginatedBookList.slice(startIndex, endIndex);
  }

  return paginatedBookList;
};
