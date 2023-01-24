# RetroGames
# Crear cuenta de almacenamiento en Azure
## Con AZ Cli
- Login en Azure
az login
- Seleccionar la cuenta 
az account set --subscription [subscriptioId|subscriptionName]
- Comprobar si el nombre está libre
az storage account check-name --name 'AccountName'
- Crear cuenta de Storage dentro de un grupo de recursos determinado.
az storage account create -n 'AccountName' -g 'ResourceGroupName' -l westeurope --sku Standard_LRS

## Desde el portal de Azure
- Abrir el portal de Azure <https://portal.azure.com>
- Crear recurso
- Seleccionar Cuenta de Almacenamiento
- Configurar Cuenta de Almacenamiento
    - Seleccionar Grupo de recursos
    - Asignar nombre único
    - Configurar tipo de redundancia
    - Seleccionar localización

PENDIENTE DE ACTUALIZAR REPO

# Subir ficheros 
## Con AZ Cli
## Con Azure Storage Explorer
