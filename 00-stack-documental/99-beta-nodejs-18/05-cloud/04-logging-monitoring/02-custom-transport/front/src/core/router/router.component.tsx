import React from 'react';
import { HashRouter, Switch, Route } from 'react-router-dom';
import { LoginScene, BookListScene, BookScene, UserScene } from 'scenes';
import { switchRoutes } from './routes';

export const RouterComponent: React.FunctionComponent = () => {
  return (
    <HashRouter>
      <Switch>
        <Route exact={true} path={switchRoutes.root} component={LoginScene} />
        <Route
          exact={true}
          path={switchRoutes.bookList}
          component={BookListScene}
        />
        <Route
          exact={true}
          path={switchRoutes.createBook}
          component={BookScene}
        />
        <Route
          exact={true}
          path={switchRoutes.editBook}
          component={BookScene}
        />
        <Route exact={true} path={switchRoutes.user} component={UserScene} />
      </Switch>
    </HashRouter>
  );
};
