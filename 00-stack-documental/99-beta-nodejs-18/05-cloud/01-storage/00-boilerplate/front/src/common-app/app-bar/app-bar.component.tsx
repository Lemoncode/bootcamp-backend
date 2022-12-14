import React from 'react';
import { useHistory } from 'react-router-dom';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import IconButton from '@material-ui/core/IconButton';
import LogoutIcon from '@material-ui/icons/ExitToApp';
import { routes } from 'core/router';
import * as api from './api';
import * as classes from './app-bar.styles';

export const AppBarComponent: React.FunctionComponent = (props) => {
  const { children } = props;
  const appBarRef = React.useRef<HTMLDivElement>(null);
  const [height, setHeight] = React.useState(0);
  const history = useHistory();

  React.useEffect(() => {
    if (appBarRef.current) {
      setHeight(appBarRef.current.clientHeight);
    }
  }, [appBarRef.current?.clientHeight]);

  const handleClick = async () => {
    await api.logout();
    history.push(routes.root);
  };

  return (
    <>
      <AppBar ref={appBarRef}>
        <Toolbar className={classes.root} variant="dense">
          <Typography variant="h6" component="h1">
            Perfil usuario
          </Typography>
          <IconButton color="inherit" onClick={handleClick}>
            <LogoutIcon fontSize="large" />
          </IconButton>
        </Toolbar>
      </AppBar>
      <div style={{ height }} />
      {children}
    </>
  );
};
