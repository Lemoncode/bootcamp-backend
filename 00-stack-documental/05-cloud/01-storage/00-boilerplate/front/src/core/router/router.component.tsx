import React from 'react';
import { HashRouter, Switch, Route } from 'react-router-dom';
import { LoginScene, UserScene } from 'scenes';
import { routes } from './routes';

export const RouterComponent: React.FunctionComponent = () => {
  return (
    <HashRouter>
      <Switch>
        <Route exact={true} path={routes.root} component={LoginScene} />
        <Route exact={true} path={routes.user} component={UserScene} />
      </Switch>
    </HashRouter>
  );
};
