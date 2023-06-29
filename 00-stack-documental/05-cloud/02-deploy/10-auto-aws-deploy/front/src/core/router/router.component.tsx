import React from 'react';
import { HashRouter, Routes, Route } from 'react-router-dom';
import { LoginScene, UserScene, BookListScene, BookScene } from '@/scenes';
import { switchRoutes } from './routes';

export const RouterComponent: React.FC = () => {
  return (
    <HashRouter>
      <Routes>
        <Route path={switchRoutes.root} element={<LoginScene />} />
        <Route path={switchRoutes.user} element={<UserScene />} />
        <Route path={switchRoutes.bookList} element={<BookListScene />} />
        <Route path={switchRoutes.createBook} element={<BookScene />} />
        <Route path={switchRoutes.editBook} element={<BookScene />} />
      </Routes>
    </HashRouter>
  );
};
