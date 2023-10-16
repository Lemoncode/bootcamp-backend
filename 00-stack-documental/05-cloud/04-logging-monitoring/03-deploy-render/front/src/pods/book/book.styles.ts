import { css } from '@emotion/css';

export const root = css`
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-areas:
    'title releaseDate'
    'author .'
    'submit submit';
  gap: 1rem;
`;

export const title = css`
  grid-area: title;
`;

export const releaseDate = css`
  grid-area: releaseDate;
`;

export const author = css`
  grid-area: author;
`;
export const submit = css`
  grid-area: submit;
  justify-self: flex-start;
`;
