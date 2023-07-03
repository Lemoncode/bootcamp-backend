import React from 'react';
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';

interface Props {
  title: string;
  children: React.ReactNode;
}

export const CardComponent: React.FC<Props> = (props) => {
  const { title, children } = props;
  return (
    <Card>
      <CardHeader title={title} />
      <CardContent>{children}</CardContent>
    </Card>
  );
};
