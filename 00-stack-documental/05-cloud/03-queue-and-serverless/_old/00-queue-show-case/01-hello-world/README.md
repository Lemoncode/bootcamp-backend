
## Running the code

To start the message broker

```bash
cd 00-show-case
./start-up-message-broker.sh
```

Open a new terminal, to start `piblisher`

```bash
cd 00-show-case
$(npm bin)/ts-node 01-hello-world/publisher.ts
```

Open a new terminal, to start `consumer`

```bash
cd 00-show-case
$(npm bin)/ts-node 01-hello-world/consumer.ts
```

## Cleanup

```bash
docker stop rabbit
```
