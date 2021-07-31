
## Running the code

To start the message broker

```bash
cd 00-show-case
./start-up-message-broker.sh
```

Open a new terminal, to start `receiver`

```bash
cd 00-show-case
$(npm bin)/ts-node 03-publisher-subscriber/receive-logs.ts
```

Open a new terminal, to start `emitter`

```bash
cd 00-show-case
$(npm bin)/ts-node 03-publisher-subscriber/emit-logs.ts Hello
```

## Cleanup

```bash
docker stop rabbit
```
