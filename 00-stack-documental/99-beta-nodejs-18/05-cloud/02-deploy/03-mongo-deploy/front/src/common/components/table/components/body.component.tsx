import React from 'react';
import { Row } from 'react-table';
import TableBody from '@material-ui/core/TableBody';
import { RowRendererProps } from '../table.vm';

interface Props<T extends object = {}> {
  rows: Row<T>[];
  prepareRow: (row: Row<T>) => void;
  rowRenderer: (props: RowRendererProps<T>) => React.ReactElement<HTMLElement>;
  className?: string;
}

export const BodyComponent: React.FunctionComponent<Props> = (props) => {
  const { rows, prepareRow, rowRenderer, className } = props;
  return (
    <TableBody className={className}>
      {rows.map((row) => {
        prepareRow(row);
        const rowProps = row.getRowProps();
        return React.cloneElement(
          rowRenderer({
            ...rowProps,
            row: row.original,
            index: row.index,
          }),
          { key: rowProps?.key }
        );
      })}
    </TableBody>
  );
};
