import React from 'react';
import Snackbar, { SnackbarOrigin } from '@mui/material/Snackbar';
import SnackbarContent from '@mui/material/SnackbarContent';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import { SnackbarContext } from './snackbar.context';
import * as classes from './snackbar.styles';

interface Props {
  autoHideDuration?: number;
  position?: SnackbarOrigin;
}

/*
Global snack bar (use one to rule them all :))

Initialize on app provider:

```typescript
import { RouterComponent } from '@/core/router';
import { SnackbarComponent, SnackbarProvider } from '@/common/components';

const App: React.FC = () => {
  return (
    <SnackbarProvider>
      <RouterComponent />
      <SnackbarComponent />
    </SnackbarProvider>
  );
};
```
```typescript
import { LoginComponent } from './login.component';
import { useSnackbarContext } from '@/common/components';

export const LoginContainer: React.FC = () => {
  const { showMessage } = useSnackbarContext();
  const navigate = useNavigate();

  const handleLogin = (login: Login) => {
    trackPromise(isValidLogin(login.user, login.password)).then(isValid =>
      isValid
        ? navigate(routes.submoduleList)
        : showMessage('text',
          'error')
    );
  };

  return <LoginComponent onLogin={handleLogin} />;
};
```

*/
export const SnackbarComponent: React.FC<Props> = (props) => {
  const { position, autoHideDuration } = props;
  const { open, onClose, options } = React.useContext(SnackbarContext);

  return (
    <Snackbar
      anchorOrigin={position}
      open={open}
      autoHideDuration={autoHideDuration}
      onClose={onClose}
    >
      <SnackbarContent
        className={classes[options.variant]}
        classes={{
          root: classes.snackbarContent,
          message: classes.message,
        }}
        message={options.message}
        action={[
          <IconButton key="close" color="inherit" onClick={onClose}>
            <CloseIcon />
          </IconButton>,
        ]}
      />
    </Snackbar>
  );
};

SnackbarComponent.defaultProps = {
  position: {
    horizontal: 'right',
    vertical: 'top',
  },
  autoHideDuration: 3000,
};
