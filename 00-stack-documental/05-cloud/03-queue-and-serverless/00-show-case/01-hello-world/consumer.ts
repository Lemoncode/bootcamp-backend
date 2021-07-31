import { messageBrokerService } from "../utils";

const { openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const queue = "hello";
    channel.assertQueue(queue, { durable: false });
    channel.consume(queue, (msg) => {
      console.log(`[x] Received ${msg.content.toString()}`);
    }, { noAck: true }); // [1]
  } catch (error) {
    console.error(error);
  }
};

run();


// [1]. if true, the broker won’t expect an acknowledgement of messages delivered to this consumer; i.e., it will dequeue messages as soon as they’ve been sent down the wire. 
