import { dbServer } from '#core/servers/index.js';
import { Comment } from './comment.model.js';

export const getCommentContext = () =>
  dbServer.db?.collection<Comment>('comments');
