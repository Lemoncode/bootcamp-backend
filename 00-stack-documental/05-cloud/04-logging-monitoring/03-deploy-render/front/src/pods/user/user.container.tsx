import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useSnackbarContext } from '@/common/components';
import { linkRoutes } from '@/core/router';
import * as api from './api';
import { createEmptyUser, User } from './user.vm';
import { mapUserFromApiToVm } from './user.mappers';
import { UserComponent } from './user.component';

interface Props {
  className?: string;
}

export const UserContainer: React.FC<Props> = (props) => {
  const { className } = props;
  const { showMessage } = useSnackbarContext();
  const navigate = useNavigate();

  const [user, setUser] = React.useState<User>(createEmptyUser());

  const handleLoad = async () => {
    try {
      const apiUser = await api.getUser();
      setUser(mapUserFromApiToVm(apiUser));
    } catch {
      navigate(linkRoutes.root);
      showMessage('Introduzca credenciales', 'error');
    }
  };

  React.useEffect(() => {
    handleLoad();
  }, []);

  return <UserComponent className={className} user={user} />;
};
