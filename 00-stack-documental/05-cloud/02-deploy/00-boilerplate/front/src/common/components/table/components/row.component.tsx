import React from 'react';
import TableRow, { TableRowProps } from '@mui/material/TableRow';
import { cx } from '@emotion/css';
import * as classes from './row.component.styles';

export interface RowProps extends TableRowProps {
  className?: string;
}

export const RowComponent = React.forwardRef<HTMLTableRowElement, RowProps>(
  (props, ref) => {
    const { className, children, ...rest } = props;
    return (
      <TableRow {...rest} ref={ref} className={cx(classes.root, className)}>
        {children}
      </TableRow>
    );
  }
);
