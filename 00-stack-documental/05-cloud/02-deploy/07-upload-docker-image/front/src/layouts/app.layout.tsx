import React from 'react';
import { AppBarComponent } from 'common-app/app-bar';
import * as classes from './app.layout.styles';

interface ChildrenProps {
  className: string;
}

interface Props {
  children: (props: ChildrenProps) => React.ReactNode;
}

export const AppLayout: React.FunctionComponent<Props> = (props) => {
  const { children } = props;

  return (
    <AppBarComponent>
      {children({ className: classes.content })}
    </AppBarComponent>
  );
};
