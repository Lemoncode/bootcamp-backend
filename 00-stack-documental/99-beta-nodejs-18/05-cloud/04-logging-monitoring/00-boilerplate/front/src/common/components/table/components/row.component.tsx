import React from 'react';
import TableRow, { TableRowProps } from '@material-ui/core/TableRow';
import { cx } from '@emotion/css';
import * as classes from './row.component.styles';

export interface RowProps extends TableRowProps {
  className?: string;
}

export const RowComponent: React.FunctionComponent<RowProps> = React.forwardRef(
  (props, ref) => {
    const { className, children, ...rest } = props;
    return (
      <TableRow {...rest} ref={ref} className={cx(classes.root, className)}>
        {children}
      </TableRow>
    );
  }
);
