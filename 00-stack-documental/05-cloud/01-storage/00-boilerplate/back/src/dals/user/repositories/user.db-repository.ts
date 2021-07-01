import { ObjectId } from 'mongodb';
import { hashPassword } from 'common/helpers';
import { userContext } from '../user.context';
import { User } from '../user.model';
import { UserRepository } from './user.repository';

export const dbRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) => {
    const user = await userContext
      .findOne({
        email,
      })
      .lean();

    const hashedPassword = await hashPassword(password, user.salt);
    return user.password === hashedPassword
      ? ({
          _id: user._id,
          email: user.email,
          role: user.role,
        } as User)
      : null;
  },
  getUserById: async (id: string) =>
    await userContext
      .findOne(
        {
          _id: new ObjectId(id),
        },
        {
          email: 1,
          role: 1,
        }
      )
      .lean(),
};
