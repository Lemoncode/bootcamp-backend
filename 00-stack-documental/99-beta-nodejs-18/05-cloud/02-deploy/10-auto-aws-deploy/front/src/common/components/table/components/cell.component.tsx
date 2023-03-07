import React from 'react';
import TableCell, { TableCellProps } from '@mui/material/TableCell';

export type CellProps = TableCellProps;

export const CellComponent: React.FunctionComponent<CellProps> = (props) => {
  const { children, className, ...rest } = props;
  return (
    <TableCell {...rest} className={className}>
      {children}
    </TableCell>
  );
};

CellComponent.defaultProps = {
  align: 'left',
};
