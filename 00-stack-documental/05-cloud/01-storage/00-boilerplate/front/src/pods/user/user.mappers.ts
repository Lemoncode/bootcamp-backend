import { envConstants } from '@/core/constants';
import * as apiModel from './api';
import * as viewModel from './user.vm';

export const mapUserFromApiToVm = (user: apiModel.User): viewModel.User => ({
  email: user.email,
  role: user.role,
  avatar:
    Boolean(envConstants.IMAGES_BASE_URL) && !user.avatar?.includes('http')
      ? `${envConstants.IMAGES_BASE_URL}/${user.avatar}`
      : user.avatar,
});
