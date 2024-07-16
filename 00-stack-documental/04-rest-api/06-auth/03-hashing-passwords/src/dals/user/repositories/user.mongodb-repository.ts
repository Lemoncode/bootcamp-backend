import { verifyHash } from '#common/helpers/index.js';
import { getUserContext } from '../user.context.js';
import { User } from '../user.model.js';
import { UserRepository } from './user.repository.js';

export const mongoDBRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) => {
    const user = await getUserContext().findOne({
      email,
    });

    return verifyHash(password, user?.password)
      ? ({
          _id: user._id,
          email: user.email,
          role: user.role,
        } as User)
      : null;
  },
};
