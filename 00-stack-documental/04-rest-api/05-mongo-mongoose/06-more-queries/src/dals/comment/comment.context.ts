import { db } from 'core/servers';
import { Comment } from './comment.model';

export const getCommentContext = () => db?.collection<Comment>('comments');
