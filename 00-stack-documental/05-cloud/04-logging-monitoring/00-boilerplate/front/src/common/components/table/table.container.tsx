import React from 'react';
import {
  useTable,
  usePagination,
  TableInstance,
  UsePaginationInstanceProps,
} from 'react-table';
import { useConfirmationDialog } from '../confirmation-dialog';
import {
  RowRendererProps,
  TableLabelProps,
  createEmptyTableLabelProps,
  TableClassesProps,
  createEmptyTableClassesProps,
  Column,
} from './table.vm';
import { TableComponent } from './table.component';
import {
  mapColumnListFromVMToCellHeaderProps,
  mapColumnListFromStringToColumn,
} from './table.mappers';

type TableProps = TableInstance & UsePaginationInstanceProps<{}>;

interface Props<T = {}> {
  columns: Column[];
  rows: T[];
  rowRenderer: (props: RowRendererProps<T>) => React.ReactElement<HTMLElement>;
  onCreate?: (event?: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onEdit?: (id: string) => void;
  onDelete?: (id: string) => void;
  labels?: TableLabelProps;
  className?: string;
  classes?: TableClassesProps;
}

export const TableContainer: React.FunctionComponent<Props> = (props) => {
  const { className } = props;
  const classes = {
    ...createEmptyTableClassesProps(),
    ...props.classes,
  };

  const labels = { ...createEmptyTableLabelProps(), ...props.labels };

  const cellHeaderPropsList = React.useMemo(
    () => mapColumnListFromVMToCellHeaderProps(props.columns),
    [props.columns]
  );
  const columns = React.useMemo(
    () =>
      mapColumnListFromStringToColumn(cellHeaderPropsList.map((c) => c.label)),
    [cellHeaderPropsList]
  );
  const data = React.useMemo(() => props.rows, [props.rows]);

  const { getTableProps, headerGroups, rows, prepareRow, page } = useTable({
    columns,
    data,
  }) as TableProps;

  const { isOpen, itemToDelete, onOpenDialog, onClose, onAccept } =
    useConfirmationDialog();

  const handleDelete = () => {
    if (props.onDelete) {
      props.onDelete(itemToDelete.id);
      onAccept();
    }
  };

  return (
    <TableComponent
      className={className}
      classes={classes}
      tableProps={{ ...getTableProps() }}
      headerGroupList={headerGroups}
      cellHeaderPropsList={cellHeaderPropsList}
      rows={rows}
      prepareRow={prepareRow}
      rowRenderer={(rowProps) =>
        props.rowRenderer({
          ...rowProps,
          onEdit: props.onEdit,
          onDelete: Boolean(props.onDelete) ? onOpenDialog : undefined,
        })
      }
      labels={labels}
      onCreate={props.onCreate}
      onDelete={Boolean(props.onDelete) ? handleDelete : undefined}
      isOpenConfirmation={isOpen}
      onCloseConfirmation={onClose}
      itemToDeleteName={itemToDelete.name}
    />
  );
};

TableContainer.defaultProps = {
  labels: createEmptyTableLabelProps(),
  classes: createEmptyTableClassesProps(),
};
