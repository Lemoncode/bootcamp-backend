## Topic exchange

Messages sent to a `topic` exchange can't have an arbitrary `routing_key` - it must be a list of words, delimited by dots. The words can be anything, but usually they specify some features connected to the message. A few valid routing key examples: "`stock.usd.nyse`", "`nyse.vmw`", "`quick.orange.rabbit`". There can be as many words in the routing key as you like, up to the limit of 255 bytes.

The binding key must also be in the same form. The logic behind the `topic` exchange is similar to a `direct` one - a message sent with a particular routing key will be delivered to all the queues that are bound with a matching binding key. However there are two important special cases for binding keys:

* <span>*</span> (star) can substitute for exactly one word.
* <span>#</span> (hash) can substitute for zero or more words.

> TODO: Add diagram

In this example, we're going to send messages which all describe animals. The messages will be sent with a routing key that consists of three words (two dots). The first word in the routing key will describe speed, second a colour and third a species: "`<speed>.<colour>.<species>`".

We created three bindings: Q1 is bound with binding key "`*.orange.*`" and Q2 with "`*.*.rabbit`" and "`lazy.#`".

These bindings can be summarised as:

* Q1 is interested in all the orange animals.
* Q2 wants to hear everything about rabbits, and everything about lazy animals.

A message with a routing key set to "`quick.orange.rabbit`" will be delivered to both queues. Message "`lazy.orange.elephant`" also will go to both of them. On the other hand "`quick.orange.fox`" will only go to the first queue, and "`lazy.brown.fox`" only to the second. "`lazy.pink.rabbit`" will be delivered to the second queue only once, even though it matches two bindings. "`quick.brown.fox`" doesn't match any binding so it will be discarded.

What happens if we break our contract and send a message with one or four words, like "`orange`" or "`quick.orange.male.rabbit`"? Well, these messages won't match any bindings and will be lost.

On the other hand "`lazy.orange.male.rabbit`", even though it has four words, will match the last binding and will be delivered to the second queue.

## Running the code

To start the message broker

```bash
cd 00-show-case
./start-up-message-broker.sh
```

To receive all the logs:

```bash
cd 00-show-case
$(npm bin)/ts-node 05-topics/receive-logs-topic.ts "#"
```

To receive all logs from the facility "`kern`"

```bash
cd 00-show-case
$(npm bin)/ts-node 05-topics/receive-logs-direct.ts "kern.*"
```

Or if you want to hear only about "`critical`" logs:

```bash
cd 00-show-case
$(npm bin)/ts-node 05-topics/receive-logs-topic.ts "*.critical"
```

You can create multiple bindings:

```bash
cd 00-show-case
$(npm bin)/ts-node 05-topics/receive-logs-topic.ts "kern.*" "*.critical"
```

And to emit a log with a routing key "`kernel.critical`" type:

```bash
cd 00-show-case
$(npm bin)/ts-node 05-topics/emit-logs-topic.ts error "kern.critical" "A critical kernel error"
```

## Cleanup

```bash
docker stop rabbit
```
