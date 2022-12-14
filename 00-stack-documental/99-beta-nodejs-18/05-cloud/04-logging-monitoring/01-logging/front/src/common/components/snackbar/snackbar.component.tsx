import React from 'react';
import Snackbar, { SnackbarOrigin } from '@material-ui/core/Snackbar';
import SnackbarContent from '@material-ui/core/SnackbarContent';
import IconButton from '@material-ui/core/IconButton';
import CloseIcon from '@material-ui/icons/Close';
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
import { RouterComponent } from 'core/router';
import { SnackbarComponent, SnackbarProvider } from 'common/components';

const App: React.FunctionComponent = () => {
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
import { useSnackbarContext } from 'common/components';

export const LoginContainer: React.FunctionComponent = () => {
  const { showMessage } = useSnackbarContext();
  const history = useHistory();

  const handleLogin = (login: Login) => {
    trackPromise(isValidLogin(login.user, login.password)).then(isValid =>
      isValid
        ? history.push(routes.submoduleList)
        : showMessage('text',
          'error')
    );
  };

  return <LoginComponent onLogin={handleLogin} />;
};
```

*/
export const SnackbarComponent: React.FunctionComponent<Props> = (props) => {
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
