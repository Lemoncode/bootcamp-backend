import React from 'react';
import { HashRouter, Routes, Route } from 'react-router-dom';
import { LoginScene, UserScene } from '@/scenes';
import { routes } from './routes';

export const RouterComponent: React.FC = () => {
  return (
    <HashRouter>
      <Routes>
        <Route path={routes.root} element={<LoginScene />} />
        <Route path={routes.user} element={<UserScene />} />
      </Routes>
    </HashRouter>
  );
};
