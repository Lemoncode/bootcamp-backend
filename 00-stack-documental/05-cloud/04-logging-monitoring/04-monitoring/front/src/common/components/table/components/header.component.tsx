import React from 'react';
import { cx } from '@emotion/css';
import { HeaderGroup } from 'react-table';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { CellHeaderProps } from '../table.vm';
import { CellComponent } from './cell.component';
import * as innerClasses from './header.styles';

interface Props {
  headerGroups: HeaderGroup[];
  cellHeaderPropsList: CellHeaderProps[];
  className?: string;
}

export const HeaderComponent: React.FunctionComponent<Props> = (props) => {
  const { headerGroups, cellHeaderPropsList, className } = props;
  return (
    <TableHead className={cx(className, innerClasses.root)}>
      {headerGroups.map((headerGroup) => (
        <TableRow {...headerGroup.getHeaderGroupProps()}>
          {headerGroup.headers.map((column, index) => {
            const cellProps = column.getHeaderProps();
            const cellHeaderProps = cellHeaderPropsList[index];

            return (
              <CellComponent
                {...cellProps}
                {...cellHeaderProps}
                className={cx(
                  innerClasses.cell,
                  cellProps.className,
                  cellHeaderProps.className
                )}
              >
                {column.render('Header')}
              </CellComponent>
            );
          })}
        </TableRow>
      ))}
    </TableHead>
  );
};
