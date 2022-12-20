import React from 'react';
import { useHistory } from 'react-router-dom';
import { useSnackbarContext } from 'common/components';
import { routes } from 'core/router';
import * as api from './api';
import { LoginComponent } from './login.component';
import { User } from './login.vm';

interface Props {
  className?: string;
}

export const LoginContainer: React.FunctionComponent<Props> = () => {
  const { showMessage } = useSnackbarContext();
  const history = useHistory();
  const handleLogin = async (user: User) => {
    try {
      await api.login(user.email, user.password);
      history.push(routes.user);
    } catch {
      showMessage('Email y/o password inválidos', 'error');
    }
  };
  return <LoginComponent onLogin={handleLogin} />;
};
