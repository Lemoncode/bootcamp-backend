import React from 'react';
import { useParams } from 'react-router-dom';
import { useSnackbarContext } from 'common/components';
import * as api from './api';
import { Book, createEmptyBook } from './book.vm';
import { BookComponent } from './book.component';
import { mapBookFromApiToVM, mapBookFromVMToApi } from './book.mappers';

interface Props {
  className?: string;
}

interface Params {
  id: string;
}

export const BookContainer: React.FunctionComponent<Props> = (props) => {
  const { className } = props;
  const { id } = useParams<Params>();
  const isEditMode = Boolean(id);
  const [book, setBook] = React.useState<Book>(createEmptyBook());
  const { showMessage } = useSnackbarContext();

  const handleLoadBook = async () => {
    const apiBook = await api.getBook(id);
    setBook(mapBookFromApiToVM(apiBook));
  };

  React.useEffect(() => {
    if (isEditMode) {
      handleLoadBook();
    }
  }, [isEditMode]);

  const handleSave = async (newBook: Book) => {
    try {
      const apiBook = mapBookFromVMToApi(newBook);
      console.log({ apiBook });
      const response = await api.saveBook(apiBook);
      if (response) {
        setBook(mapBookFromApiToVM(response));
      }
      showMessage('Libro guardado correctamente', 'success');
    } catch {
      showMessage('No se ha podido guardar el libro', 'error');
    }
  };

  return (
    <BookComponent className={className} book={book} onSave={handleSave} />
  );
};
