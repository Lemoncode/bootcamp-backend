import { getSignedUrl } from '@aws-sdk/s3-request-presigner';
import { GetObjectCommand } from '@aws-sdk/client-s3';
import { s3Client } from 'core/clients';
import * as model from 'dals/user';
import * as apiModel from './user.api-model';

// TODO: Move to env variable
const bucket = 'bucket-name';

const mapAvatar = async (avatar: string): Promise<string> => {
  const command = new GetObjectCommand({
    Bucket: bucket,
    Key: avatar,
  });
  const expiresIn = 60 * 60 * 24; // 1 day expiration time.
  return await getSignedUrl(s3Client, command, { expiresIn });
};

export const mapUserFromModelToApi = async (
  user: model.User
): Promise<apiModel.User> => {
  const avatar = await mapAvatar(user.avatar);
  return {
    email: user.email,
    role: user.role,
    avatar,
  };
};
