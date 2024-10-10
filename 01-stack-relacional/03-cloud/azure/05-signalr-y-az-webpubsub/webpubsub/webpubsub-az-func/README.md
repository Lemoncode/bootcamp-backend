## Cómo ejecutarlo en local

En primer lugar necesitas establecer la siguiente variable de entorno con la cadena de conexión de tu servicio de Azure Web PubSub:

```bash
export WebPubSubConnectionString="Endpoint=https://lastheroes.webpubsub.azure.com;AccessKey=vrVldjUg8y379ssQhqMfWAYQsQA3+tkYR/TtxQHrWK4=;Version=1.0;"
```

Y ahora para poder conectarte a tu servicio de Azure Web PubSub necesitas establecer la siguiente variable de entorno con el nombre de tu hub:

```bash
SUBSCRIPTION_ID=$(az account show --query id -o tsv)
RESOURCE_GROUP=tour-of-heroes

awps-tunnel run --hub notification --endpoint https://lastheroes.webpubsub.azure.com -s $SUBSCRIPTION_ID -g $RESOURCE_GROUP
```