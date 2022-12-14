import React from 'react';
import EditIcon from '@material-ui/icons/Edit';
import DeleteIcon from '@material-ui/icons/Delete';
import IconButton from '@material-ui/core/IconButton';
import {
  RowRendererProps,
  RowComponent,
  CellComponent,
} from 'common/components';
import { Book } from '../book-list.vm';

type Props = RowRendererProps<Book>;

export const BookRowComponent: React.FunctionComponent<Props> = (props) => {
  const { row, onEdit, onDelete } = props;
  return (
    <RowComponent>
      <CellComponent>{row.title}</CellComponent>
      <CellComponent>{row.author}</CellComponent>
      <CellComponent>{row.releaseDate}</CellComponent>
      <CellComponent align="center">
        <IconButton onClick={() => onEdit(row.id)}>
          <EditIcon />
        </IconButton>
        <IconButton onClick={() => onDelete({ id: row.id, name: row.title })}>
          <DeleteIcon />
        </IconButton>
      </CellComponent>
    </RowComponent>
  );
};
