import { Column } from 'react-table';
import * as viewModel from './table.vm';
import { mapToCollection } from 'common/mappers';

const mapColumnFromVMToCellHeaderProps = (
  column: viewModel.Column
): viewModel.CellHeaderProps =>
  typeof column === 'string' ? { label: column } : column;

export const mapColumnListFromVMToCellHeaderProps = (
  columns: viewModel.Column[]
): viewModel.CellHeaderProps[] =>
  mapToCollection(columns, mapColumnFromVMToCellHeaderProps);

const mapColumnFromStringToColumn = (column: string): Column => ({
  accessor: column,
  Header: column,
});

export const mapColumnListFromStringToColumn = (columns: string[]): Column[] =>
  mapToCollection(columns, mapColumnFromStringToColumn);
