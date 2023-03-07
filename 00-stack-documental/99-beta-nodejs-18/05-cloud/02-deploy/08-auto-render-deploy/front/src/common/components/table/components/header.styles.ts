import { css } from '@emotion/css';
import { theme } from 'core/theme';

export const root = css`
  background-color: ${theme.palette.primary.main};
`;

export const cell = css`
  color: ${theme.palette.primary.contrastText};
`;
