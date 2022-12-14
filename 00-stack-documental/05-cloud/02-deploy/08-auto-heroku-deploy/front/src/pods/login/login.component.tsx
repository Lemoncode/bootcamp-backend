import React from 'react';
import { CardComponent, LoginFormComponent } from './components';
import { User } from './login.vm';

interface Props {
  onLogin: (user: User) => void;
}

export const LoginComponent: React.FunctionComponent<Props> = (props) => {
  const { onLogin } = props;
  return (
    <CardComponent title="Bienvenido">
      <LoginFormComponent onLogin={onLogin} />
    </CardComponent>
  );
};
