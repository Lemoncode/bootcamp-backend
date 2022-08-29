## Running the code

To start the message broker

```bash
cd 00-show-case
./start-up-message-broker.sh
```

Open a new terminal, to start `worker`

```bash
cd 00-show-case
$(npm bin)/ts-node 02-work-queues/worker.ts
```

Open a new terminal, to start `task publisher`

```bash
cd 00-show-case
$(npm bin)/ts-node 02-work-queues/new-task.ts Hello...
```

## Round Robin

Start two workers and run:

```bash
cd 00-show-case
./multiple-tasks.sh
```


## Cleanup

```bash
docker stop rabbit
```


## Update worker for acknowledgment

Update `worker.ts`

```diff
import { messageBrokerService, invokeWithDelay } from "../utils";

const { openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const queue = "task_queue";
    await channel.assertQueue(queue, { durable: true });

    channel.consume(
      queue,
-     async ({content}) => {
+     async (msg) => {
+       const { content } = msg;
        const seconds = content.toString().split(".").length - 1;
        console.log(`[x] Received ${content.toString()}`);

        await invokeWithDelay(seconds * 1_000, () => {
          console.log("[x] Done");
+         channel.ack(msg);
        });
      },
-     { noAck: true }
+     { noAck: false }
    );
  } catch (error) {
    console.error(error);
  }
};

run();

```

## Update worker to do not get another message until it finishes current one

```diff
import { messageBrokerService, invokeWithDelay } from "../utils";

const { openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const queue = "task_queue";
    await channel.assertQueue(queue, { durable: true });
    
+   channel.prefetch(1);

    channel.consume(
      queue,
      async (msg) => {
        const { content } = msg;
        const seconds = content.toString().split(".").length - 1;
        console.log(`[x] Received ${content.toString()}`);

        await invokeWithDelay(seconds * 1_000, () => {
          console.log("[x] Done");
          channel.ack(msg);
        });
      },
      { noAck: false }
    );
  } catch (error) {
    console.error(error);
  }
};

run();

```
