import { generatePath as routerGeneratePath } from 'react-router-dom';

type Param = {
  [x: string]: string;
};

export const generatePath = (route: string, params: Param) => {
  const protectedParams = Object.entries(params)
    .map(([key, value]) => (Boolean(value) ? { [key]: value } : { [key]: '0' }))
    .reduce((total, param) => ({ ...total, ...param }), {});

  return routerGeneratePath(route, protectedParams);
};
