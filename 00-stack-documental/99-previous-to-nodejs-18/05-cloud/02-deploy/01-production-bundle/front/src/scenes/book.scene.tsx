import React from 'react';
import { AppLayout } from 'layouts';
import { BookContainer } from 'pods/book';

export const BookScene: React.FunctionComponent = () => {
  return (
    <AppLayout>
      {({ className }) => <BookContainer className={className} />}
    </AppLayout>
  );
};
