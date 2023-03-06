import React from 'react';
import StyledEngineProvider from '@mui/material/StyledEngineProvider';
import ThemeProvider from '@mui/material/styles/ThemeProvider';
import CssBaseline from '@mui/material/CssBaseline';
import { theme } from './theme';

interface Props {
  children: React.ReactNode;
}

export const ThemeProviderComponent: React.FC<Props> = (props) => {
  const { children } = props;

  return (
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </StyledEngineProvider>
  );
};
