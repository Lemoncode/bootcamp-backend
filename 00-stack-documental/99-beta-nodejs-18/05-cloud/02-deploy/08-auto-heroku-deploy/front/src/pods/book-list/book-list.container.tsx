import React from 'react';
import { useHistory } from 'react-router-dom';
import { AxiosError } from 'axios';
import { useSnackbarContext } from 'common/components';
import * as api from './api';
import { BookListComponent } from './book-list.component';
import { Book } from './book-list.vm';
import { linkRoutes } from 'core/router';
import { mapBookListFromApiToVM } from './book-list.mappers';

interface Props {
  className?: string;
}

export const BookListContainer: React.FunctionComponent<Props> = (props) => {
  const { className } = props;
  const [bookList, setBookList] = React.useState<Book[]>([]);
  const { showMessage } = useSnackbarContext();
  const history = useHistory();

  const handleError = (error) => {
    const { response } = error as AxiosError;
    if (response.status === 403 || response.status === 401) {
      history.push(linkRoutes.root);
      showMessage('Introduzca credenciales apropiados', 'error');
    }
  };

  const handleLoad = async () => {
    try {
      const apiBookList = await api.getBookList();
      setBookList(mapBookListFromApiToVM(apiBookList));
    } catch (error) {
      handleError(error);
    }
  };
  React.useEffect(() => {
    handleLoad();
  }, []);

  const handleCreate = () => {
    history.push(linkRoutes.createBook);
  };

  const handleEdit = (id: string) => {
    history.push(linkRoutes.editBook({ id }));
  };

  const handleDelete = async (id: string) => {
    try {
      await api.deleteBook(id);
      await handleLoad();
    } catch (error) {
      handleError(error);
    }
  };

  return (
    <BookListComponent
      className={className}
      bookList={bookList}
      onCreate={handleCreate}
      onEdit={handleEdit}
      onDelete={handleDelete}
    />
  );
};
