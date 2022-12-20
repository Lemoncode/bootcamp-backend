import { css } from '@emotion/css';
import { theme } from 'core/theme';

export const success = css`
  &.MuiSnackbarContent-root {
    background-color: ${theme.palette.success.main};
  }
`;

export const error = css`
  &.MuiSnackbarContent-root {
    background-color: ${theme.palette.error.dark};
  }
`;

export const info = css`
  &.MuiSnackbarContent-root {
    background-color: ${theme.palette.info.main};
  }
`;

export const warning = css`
  &.MuiSnackbarContent-root {
    background-color: ${theme.palette.warning.main};
  }
`;

export const snackbarContent = css`
  align-items: flex-start;
`;

export const message = css`
  align-self: center;
  white-space: pre;
`;
