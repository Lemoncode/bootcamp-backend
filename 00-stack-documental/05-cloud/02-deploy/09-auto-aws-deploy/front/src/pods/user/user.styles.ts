import { css } from '@emotion/css';
import { theme } from 'core/theme';

export const root = css`
  display: grid;
  column-gap: 2rem;
  row-gap: 1rem;
  grid-template-columns: auto 1fr;
  grid-template-areas:
    'avatar email'
    'avatar role'
    'avatar .';
`;

export const avatar = css`
  grid-area: avatar;
  background-color: ${theme.palette.grey[400]};
  width: ${theme.spacing(50)}px;
  height: ${theme.spacing(50)}px;
`;

export const email = css`
  grid-area: email;
  align-self: flex-end;
`;

export const role = css`
  grid-area: role;
`;
