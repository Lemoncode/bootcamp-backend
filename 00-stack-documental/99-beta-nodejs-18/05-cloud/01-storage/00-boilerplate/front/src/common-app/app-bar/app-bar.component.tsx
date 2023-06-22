import React from 'react';
import { useNavigate } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import LogoutIcon from '@mui/icons-material/ExitToApp';
import { routes } from '@/core/router';
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

  React.useEffect(() => {
    if (appBarRef.current) {
      setHeight(appBarRef.current.clientHeight);
    }
  }, [appBarRef.current?.clientHeight]);

  const handleClick = async () => {
    await api.logout();
    navigate(routes.root);
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
