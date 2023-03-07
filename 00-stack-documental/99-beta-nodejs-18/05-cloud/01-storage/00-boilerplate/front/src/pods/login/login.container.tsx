import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useSnackbarContext } from 'common/components';
import { routes } from 'core/router';
import * as api from './api';
import { LoginComponent } from './login.component';
import { User } from './login.vm';

interface Props {
  className?: string;
}

export const LoginContainer: React.FC<Props> = () => {
  const { showMessage } = useSnackbarContext();
  const navigate = useNavigate();
  const handleLogin = async (user: User) => {
    try {
      await api.login(user.email, user.password);
      navigate(routes.user);
    } catch {
      showMessage('Email y/o password inválidos', 'error');
    }
  };
  return <LoginComponent onLogin={handleLogin} />;
};
