import * as apiModel from './api';
import * as viewModel from './user.vm';

export const mapUserFromApiToVm = (user: apiModel.User): viewModel.User => ({
  email: user.email,
  role: user.role,
  avatar: user.avatar,
});
