import { ObjectId } from 'mongodb';
import { Role } from '#core/models/index.js';

export interface User {
  _id: ObjectId;
  email: string;
  password: string;
  role: Role;
}
