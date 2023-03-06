import { css } from '@emotion/css';
import { theme } from 'core/theme';

export const container = css`
  border-radius: 3px;
  display: grid;
  grid-template-columns: 1fr;
  column-gap: 1rem;
  row-gap: 1rem;
  grid-template-areas:
    'buttons'
    'table'
    'table'
    'pagination';

  @media (min-width: ${theme.breakpoints.values.md}px) {
    grid-template-columns: 1fr 1fr;
    grid-template-areas:
      '. buttons'
      'table table'
      'table table'
      'pagination pagination';
  }
`;

export const buttons = css`
  grid-area: buttons;
  align-self: end;
  justify-self: end;
`;

export const table = css`
  grid-area: table;
`;

export const pagination = css`
  grid-area: pagination;
  justify-self: center;
`;
