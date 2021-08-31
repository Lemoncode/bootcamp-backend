
## Running the code

To start the message broker

```bash
cd 00-show-case
./start-up-message-broker.sh
```

Open a new terminal, to start `receiver`, where severity can have the following values: `info`, `warning` and `error`.

```bash
cd 00-show-case
$(npm bin)/ts-node 04-routing/receive-logs-direct.ts <severity>
```

To receive all kind of messages

```bash
cd 00-show-case
$(npm bin)/ts-node 04-routing/receive-logs-direct.ts info warning error
```

Open a new terminal, to start `emitter`

```bash
cd 00-show-case
$(npm bin)/ts-node 04-routing/emit-logs-direct.ts error "Run. Run. Or it will explode."
```

## Cleanup

```bash
docker stop rabbit
```
