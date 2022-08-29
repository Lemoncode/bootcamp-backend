#!/bin/bash
if [[ ! ($(which docker) && $(docker --version)) ]]; then
    echo "Install docker to run RabbitMQ message broker"
    exit 1
fi

if [[ ! "$(docker ps -q -f name=rabbit)" ]]; then
    if [[ "$(docker ps -aq -f status=exited -f name=rabbit)" ]]; then
        # cleanup
        docker rm rabbit
    fi
    # run your container
    docker run --rm -d \
         --name rabbit \
         --hostname backgammon-message-broker \
         -p 15672:15672 \
         -p 5672:5672 \
         rabbitmq:3-management  
fi

# wait to TCP connection ready
while ! nc -z localhost 5672; do sleep 3; done

sleep 10
