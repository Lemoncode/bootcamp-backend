import React from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import IconButton from '@material-ui/core/IconButton';
import HomeIcon from '@material-ui/icons/Home';
import UserIcon from '@material-ui/icons/Person';
import LogoutIcon from '@material-ui/icons/ExitToApp';
import { linkRoutes } from 'core/router';
import * as api from './api';
import * as classes from './app-bar.styles';

export const AppBarComponent: React.FunctionComponent = (props) => {
  const { children } = props;
  const appBarRef = React.useRef<HTMLDivElement>(null);
  const [height, setHeight] = React.useState(0);
  const history = useHistory();
  const location = useLocation();

  React.useEffect(() => {
    if (appBarRef.current) {
      setHeight(appBarRef.current.clientHeight);
    }
  }, [appBarRef.current?.clientHeight]);

  const handleClick = async () => {
    await api.logout();
    history.push(linkRoutes.root);
  };

  return (
    <>
      <AppBar ref={appBarRef}>
        <Toolbar className={classes.root} variant="dense">
          <div className={classes.brandContainer}>
            <IconButton
              color="inherit"
              onClick={() => {
                history.push(linkRoutes.bookList);
              }}
            >
              <HomeIcon fontSize="large" />
            </IconButton>
            <Typography variant="h6" component="h1">
              {location.pathname === linkRoutes.user
                ? 'Perfil usuario'
                : 'Tienda de libros'}
            </Typography>
          </div>
          <div>
            <IconButton
              color="inherit"
              onClick={() => {
                history.push(linkRoutes.user);
              }}
            >
              <UserIcon fontSize="large" />
            </IconButton>
            <IconButton color="inherit" onClick={handleClick}>
              <LogoutIcon fontSize="large" />
            </IconButton>
          </div>
        </Toolbar>
      </AppBar>
      <div style={{ height }} />
      {children}
    </>
  );
};
