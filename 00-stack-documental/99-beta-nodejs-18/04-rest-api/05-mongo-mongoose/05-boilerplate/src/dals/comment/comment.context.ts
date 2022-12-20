import { db } from '#core/servers/index.js';
import { Comment } from './comment.model.js';

export const getCommentContext = () => db?.collection<Comment>('comments');
