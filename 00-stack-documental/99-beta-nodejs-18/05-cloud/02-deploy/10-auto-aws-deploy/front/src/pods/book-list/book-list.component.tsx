import React from 'react';
import { TableContainer } from 'common/components';
import { Book } from './book-list.vm';
import { BookRowComponent } from './components';

interface Props {
  bookList: Book[];
  onCreate: () => void;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  className?: string;
}

export const BookListComponent: React.FunctionComponent<Props> = (props) => {
  const { className, bookList, onCreate, onEdit, onDelete } = props;
  return (
    <TableContainer
      className={className}
      columns={[
        'Titulo',
        'Autor',
        'Fecha Publicación',
        { label: 'Comandos', align: 'center' },
      ]}
      rows={bookList}
      rowRenderer={BookRowComponent}
      labels={{
        createButton: 'Añadir libro',
        deleteConfirmationDialog: {
          title: 'Borrar libro',
          content: ({itemName}) => (
            <p>
              ¿Está seguro de borrar <strong>{itemName}</strong>?
            </p>
          ),
          closeButton: 'Cancelar',
          acceptButton: 'Aceptar',
        },
      }}
      onCreate={onCreate}
      onEdit={onEdit}
      onDelete={onDelete}
    />
  );
};
