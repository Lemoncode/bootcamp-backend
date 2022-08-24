import { ObjectId } from 'mongodb';
import { Role } from 'common-app/models';

export interface User {
  _id: ObjectId;
  email: string;
  password: string;
  salt: string;
  role: Role;
  avatar: string;
}
