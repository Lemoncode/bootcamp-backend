#Para ver todos los comandos que ofrece Azure CLI
az

#Para iniciar sesión en la suscripción sobre la que quieres operar
az login

#Variables
RESOURCE_GROUP="Tour-Of-Heroes"
STORAGE_NAME="<YOUR_STORAGE_ACCOUNT_NAME>"
LOCATION="northeurope"

#Crear una cuenta de almacenamiento dentro del grupo de recursos Tour-Of-Heroes
az storage account create --resource-group $RESOURCE_GROUP --name $STORAGE_NAME --location $LOCATION

#Crear el contenedor de los superheroes
az storage container create --name heroes --account-name $STORAGE_NAME --public-access blob

#Crear el contenedor de los alter egos
az storage container create --name alteregos --account-name $STORAGE_NAME

#Subir alteregos
az storage blob upload-batch --destination alteregos --source src/assets/alteregos/. --account-name $STORAGE_NAME

#Los héroes los subimos con Azure Storage Explorer

#Using Azurite
#Azurite is a free, open source, cross-platform, cloud-based development environment for building, testing, and deploying applications.
npm install -g azurite

#Start azurite
azurite --location c:\azurite --loose

#Connection string for azurite
$AZURITE_CONNECTION_STRING="DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;"

#Create containers
az storage container create -n heroes --public-access blob --connection-string $AZURITE_CONNECTION_STRING
az storage blob upload-batch --destination heroes --source src/assets/heroes/. --connection-string $AZURITE_CONNECTION_STRING

az storage container create -n alteregos --connection-string $AZURITE_CONNECTION_STRING
az storage blob upload-batch --destination alteregos --source src/assets/alteregos/. --connection-string $AZURITE_CONNECTION_STRING

#Configure CORS settings
az storage cors add --origin 'http://localhost:4200' --methods OPTIONS, PUT --services b --connection-string $AZURITE_CONNECTION_STRING

