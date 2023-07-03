import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import HomeIcon from '@mui/icons-material/Home';
import UserIcon from '@mui/icons-material/Person';
import LogoutIcon from '@mui/icons-material/ExitToApp';
import { linkRoutes } from '@/core/router';
import * as api from './api';
import * as classes from './app-bar.styles';

interface Props {
  children: React.ReactNode;
}

export const AppBarComponent: React.FC<Props> = (props) => {
  const { children } = props;
  const appBarRef = React.useRef<HTMLDivElement>(null);
  const [height, setHeight] = React.useState(0);
  const navigate = useNavigate();
  const location = useLocation();

  React.useEffect(() => {
    if (appBarRef.current) {
      setHeight(appBarRef.current.clientHeight);
    }
  }, [appBarRef.current?.clientHeight]);

  const handleClick = async () => {
    await api.logout();
    navigate(linkRoutes.root);
  };

  return (
    <>
      <AppBar ref={appBarRef}>
        <Toolbar className={classes.root} variant="dense">
          <div className={classes.brandContainer}>
            <IconButton
              color="inherit"
              onClick={() => {
                navigate(linkRoutes.bookList);
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
                navigate(linkRoutes.user);
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
