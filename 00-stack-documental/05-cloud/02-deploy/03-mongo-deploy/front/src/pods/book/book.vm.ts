export interface Book {
  id: string;
  title: string;
  releaseDate: string;
  author: string;
}

export const createEmptyBook = (): Book => ({
  id: undefined,
  title: '',
  releaseDate: '',
  author: '',
});
