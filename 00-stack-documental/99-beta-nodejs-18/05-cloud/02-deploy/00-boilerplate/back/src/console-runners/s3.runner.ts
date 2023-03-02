import {
  S3Client,
  ListObjectsCommand,
  GetObjectCommand,
  PutObjectCommand,
} from '@aws-sdk/client-s3';
import { getSignedUrl } from '@aws-sdk/s3-request-presigner';

export const run = async () => {
  try {
    const client = new S3Client({ region: 'eu-west-3' });
    const bucket = 'lab-to-rm';
    const fileName = 'user-avatar-in-s3.png';
    const command = new GetObjectCommand({
      Bucket: bucket,
      Key: fileName,
    });
    const url = await getSignedUrl(client, command);
    console.log({ url });
  } catch (error) {
    console.error(error);
  }
};
