import React from 'react';
import { useNavigate } from 'react-router-dom';
import { AxiosError } from 'axios';
import { useSnackbarContext } from '@/common/components';
import { linkRoutes } from '@/core/router';
import * as api from './api';
import { BookListComponent } from './book-list.component';
import { Book } from './book-list.vm';
import { mapBookListFromApiToVM } from './book-list.mappers';

interface Props {
  className?: string;
}

export const BookListContainer: React.FunctionComponent<Props> = (props) => {
  const { className } = props;
  const [bookList, setBookList] = React.useState<Book[]>([]);
  const { showMessage } = useSnackbarContext();
  const navigate = useNavigate();

  const handleError = (error) => {
    const { response } = error as AxiosError;
    if (response.status === 403 || response.status === 401) {
      navigate(linkRoutes.root);
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
    navigate(linkRoutes.createBook);
  };

  const handleEdit = (id: string) => {
    navigate(linkRoutes.editBook({ id }));
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
