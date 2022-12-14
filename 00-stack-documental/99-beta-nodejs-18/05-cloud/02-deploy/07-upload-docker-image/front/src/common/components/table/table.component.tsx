import React from 'react';
import { cx } from '@emotion/css';
import { TableProps, HeaderGroup, Row } from 'react-table';
import AddIcon from '@material-ui/icons/Add';
import Paper from '@material-ui/core/Paper';
import TableContainer from '@material-ui/core/TableContainer';
import Table from '@material-ui/core/Table';
import Typography from '@material-ui/core/Typography';
import { ConfirmationDialogComponent } from '../confirmation-dialog';
import { IconButtonComponent } from '../icon-button';
import { HeaderComponent, BodyComponent } from './components';
import {
  RowRendererProps,
  TableLabelProps,
  TableClassesProps,
  CellHeaderProps,
} from './table.vm';
import * as innerClasses from './table.styles';

interface Props<T extends object = {}> {
  tableProps: TableProps;
  headerGroupList: HeaderGroup<T>[];
  cellHeaderPropsList: CellHeaderProps[];
  rows: Row<T>[];
  prepareRow: (row: Row<T>) => void;
  rowRenderer: (props: RowRendererProps<T>) => React.ReactElement<HTMLElement>;
  labels: TableLabelProps;
  onCreate?: (event?: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onDelete?: () => void;
  itemToDeleteName?: string;
  isOpenConfirmation?: boolean;
  onCloseConfirmation?: () => void;
  className?: string;
  classes?: TableClassesProps;
}

export const TableComponent: React.FunctionComponent<Props> = (props) => {
  const {
    tableProps,
    headerGroupList,
    cellHeaderPropsList,
    rows,
    prepareRow,
    rowRenderer,
    onCreate,
    onDelete,
    itemToDeleteName,
    isOpenConfirmation,
    onCloseConfirmation,
    labels,
    className,
    classes,
  } = props;

  return (
    <div className={cx(className, classes.root)}>
      <div className={cx(innerClasses.container, classes.container)}>
        <div className={cx(innerClasses.buttons, classes.buttons)}>
          {onCreate && (
            <IconButtonComponent
              className={classes.createButton}
              onClick={onCreate}
              tooltip={labels.createButton}
            >
              <AddIcon />
            </IconButtonComponent>
          )}
        </div>

        <TableContainer component={Paper} className={innerClasses.table}>
          <Table
            {...tableProps}
            className={cx(tableProps.className, classes.table)}
          >
            <HeaderComponent
              className={classes.tableHeader}
              headerGroups={headerGroupList}
              cellHeaderPropsList={cellHeaderPropsList}
            />
            <BodyComponent
              className={classes.tableBody}
              rows={rows}
              prepareRow={prepareRow}
              rowRenderer={rowRenderer}
            />
          </Table>
        </TableContainer>
        {onDelete && (
          <ConfirmationDialogComponent
            className={classes.deleteConfirmationDialog}
            isOpen={isOpenConfirmation}
            onAccept={onDelete}
            onClose={onCloseConfirmation}
            title={labels.deleteConfirmationDialog.title}
            labels={{
              closeButton: labels.deleteConfirmationDialog.closeButton,
              acceptButton: labels.deleteConfirmationDialog.acceptButton,
            }}
          >
            <Typography variant="body1">
              {labels.deleteConfirmationDialog.content({
                itemName: itemToDeleteName,
              })}
            </Typography>
          </ConfirmationDialogComponent>
        )}
      </div>
    </div>
  );
};
