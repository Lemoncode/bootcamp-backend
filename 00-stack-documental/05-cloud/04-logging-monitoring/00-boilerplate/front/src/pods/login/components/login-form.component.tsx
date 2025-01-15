import React from 'react';
import { useForm } from 'react-hook-form';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { User, createEmptyUser } from '../login.vm';
import * as classes from './login-form.styles';

interface Props {
  onLogin: (user: User) => void;
}

export const LoginFormComponent: React.FC<Props> = (props) => {
  const { onLogin } = props;
  const { register, handleSubmit } = useForm({
    defaultValues: createEmptyUser(),
  });
  return (
    <form className={classes.root} onSubmit={handleSubmit(onLogin)}>
      <TextField
        inputProps={{ ...register('email') }}
        label="Email"
        fullWidth={true}
        autoComplete="off"
      />
      <TextField
        inputProps={{ ...register('password') }}
        type="password"
        label="Contraseña"
        fullWidth={true}
      />
      <Button type="submit" variant="contained" color="primary">
        Entrar
      </Button>
    </form>
  );
};
