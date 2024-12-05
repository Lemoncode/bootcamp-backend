import { ObjectId } from 'mongodb';
import { hashPassword } from '#common/helpers/index.js';
import { getUserContext } from '../user.context.js';
import { User } from '../user.model.js';
import { UserRepository } from './user.repository.js';

export const dbRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) => {
    const user = await getUserContext().findOne({
      email,
    });

    const hashedPassword = await hashPassword(password, user?.salt);
    return user?.password === hashedPassword
      ? ({
          _id: user._id,
          email: user.email,
          role: user.role,
        } as User)
      : null;
  },
  getUserById: async (id: string) =>
    await getUserContext().findOne(
      {
        _id: new ObjectId(id),
      },
      {
        projection: {
          email: 1,
          role: 1,
          avatar: 1,
        },
      }
    ),
};
