import React from 'react';
import { AppLayout } from 'layouts';
import { UserContainer } from 'pods/user';

export const UserScene: React.FunctionComponent = () => {
  return (
    <AppLayout>
      {({ className }) => <UserContainer className={className} />}
    </AppLayout>
  );
};
