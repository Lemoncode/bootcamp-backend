import amqp from "amqplib";

const url = "amqp://guest:guest@localhost:5672";

export const messageBrokerService = () => {
  let connection: amqp.Connection;

  return {
    openChannel: async (): Promise<amqp.Channel> => {
      connection = await amqp.connect(url);
      return connection.createChannel();
    },
    close: async (): Promise<void> => {
      await connection.close();
    },
  };
};

