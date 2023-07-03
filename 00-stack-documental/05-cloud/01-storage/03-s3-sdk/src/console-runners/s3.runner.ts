import {
  S3Client,
  ListObjectsCommand,
  GetObjectCommand,
  PutObjectCommand,
} from '@aws-sdk/client-s3';
import fs from 'fs';
import path from 'path';

export const run = async () => {
  try {
    const client = new S3Client({ region: 'eu-west-3' });
    const bucket = 'bucket-name';
    const fileName = 'user-avatar-in-s3.png';
    const imageStream = fs.createReadStream(path.resolve('./', fileName));
    const command = new PutObjectCommand({
      Bucket: bucket,
      Key: fileName,
      Body: imageStream,
    });
    const data = await client.send(command);
    console.log({ data });
  } catch (error) {
    console.error(error);
  }
};
