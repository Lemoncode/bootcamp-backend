import React from 'react';
import { SnackbarProvider, SnackbarComponent } from '@/common/components';
import { RouterComponent } from '@/core/router';
import { ThemeProviderComponent } from '@/core/theme';

const App: React.FC = () => {
  return (
    <ThemeProviderComponent>
      <SnackbarProvider>
        <RouterComponent />
        <SnackbarComponent />
      </SnackbarProvider>
    </ThemeProviderComponent>
  );
};

export default App;
