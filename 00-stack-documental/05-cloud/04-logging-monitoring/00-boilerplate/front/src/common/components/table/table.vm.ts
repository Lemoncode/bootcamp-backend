import React from 'react';
import { Lookup } from 'common/models';
import { CellProps } from './components';
import {
  ConfirmationDialogLabelProps,
  createEmptyConfirmationDialogLabelProps,
} from '../confirmation-dialog';

export interface CellHeaderProps extends CellProps {
  label: string;
}

export type Column = string | CellHeaderProps;

export interface RowRendererProps<T = {}> {
  row: T;
  index: number;
  key: string | number;
  onEdit?: (id: string) => void;
  onDelete?: (lookup: Lookup) => void;
}

interface DeleteConfirmationDialogLabelProps
  extends ConfirmationDialogLabelProps {
  title: string | React.ReactNode;
  content: (props: { itemName: string }) => React.ReactNode;
}

const createEmptyDeleteConfirmationDialogLabelProps = (): DeleteConfirmationDialogLabelProps => ({
  ...createEmptyConfirmationDialogLabelProps(),
  title: '',
  content: undefined,
});

export interface TableLabelProps {
  createButton?: string;
  deleteConfirmationDialog?: DeleteConfirmationDialogLabelProps;
}

export const createEmptyTableLabelProps = (): TableLabelProps => ({
  createButton: '',
  deleteConfirmationDialog: createEmptyDeleteConfirmationDialogLabelProps(),
});

export interface TableClassesProps {
  root?: string;
  container?: string;
  searchBar?: string;
  buttons?: string;
  createButton?: string;
  table?: string;
  tableHeader?: string;
  tableBody?: string;
  pagination?: string;
  deleteConfirmationDialog?: string;
}

export const createEmptyTableClassesProps = (): TableClassesProps => ({
  root: '',
  container: '',
  searchBar: '',
  buttons: '',
  createButton: '',
  table: '',
  tableHeader: '',
  tableBody: '',
  pagination: '',
  deleteConfirmationDialog: '',
});
