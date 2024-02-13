[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/0gis0/tour-of-heroes-angular)

# Aplicación de ejemplo en Angular: Tour Of Heroes

En esta versión del proyecto, se crea añade [un nuevo componente de Angular](https://angular.io/tutorial/toh-pt3), llamado file-upload que puedes encontrar en **app/file-upload**. Este es el HTML del componente:

```
<div class="file-upload-wrapper">
    <label class="custom-file-upload">
        <input type="file" (change)="onImageChange($event)" />
        Change image
    </label>
    <button [disabled]="imgName == '' ? true : null" (click)="upload()">Apply image</button>
</div>
```

Lo que nos va a permitir es que podamos subir una imagen nueva para nuestros alter egos y que esta se actualice en la página.

Este componente tiene un TypeScript que hace lo siguiente:

```
  async upload() {

    //Preparamos el nombre de la imagen para que luego sepamos recuperarla
    const finalImageName = `${this.hero.alterEgo.replace(' ', '-').toLowerCase()}${this.imgName.slice(this.imgName.lastIndexOf('.'))}`;

    console.log(`final image name: ${finalImageName}`);

    //Recuperamos un token para poder subir la imagen a Azure Storage
    this.heroService.getSasToken(finalImageName).subscribe(async (uriSas) => {
      console.log(`uriSas: ${uriSas}`);

      //Creamos un cliente de blob storage con la URL y el token
      const blobClient = new BlobServiceClient(uriSas);
      //Recuperamos el contenedor donde se almacenará la imagen
      const containerClient = blobClient.getContainerClient(environment.containerName);

      console.log(`Final image name: ${finalImageName}`);

      //Creamos la referencia de donde estará nuestra nueva imagen
      const blobFile = containerClient.getBlockBlobClient(finalImageName);

      //La subimos al contenedor
      await blobFile.uploadData(this.image, {
        concurrency: 20,
        onProgress: (ev) => console.log(ev),
        blobHTTPHeaders: { blobContentType: this.fileType }
      });

      //Avisamos a Angular de que hemos cambiado la imagen, simplemente para que la recargue
      this.messageEvent.emit('newAlterEgoImage');

    });
```

Esta función llama a nuestra API para recuperar el SAS para poder subir la imagen (si no daría un 403). La última linea del código emite un evento para que el componente padre sepa que hemos cambiado la imagen y simplemente haga una nueva llamada para recargarla.

## Cómo lo ejecuto

**IMPORTANTE**: Antes de ejecutar este proyecto necesitas en local Azurite ejecutándose. Para ello puedes lanzar el siguiente comando:

```bash
docker run \
--name azurite \
-p 10000:10000 \
-p 10001:10001 \
mcr.microsoft.com/azure-storage/azurite
```

Una vez lo tengas es importante que configures CORS para que la API pueda subir imágenes. Para ello puedes ejecutar el siguiente comando:

```bash
az storage cors add \
--methods GET POST PUT DELETE \
--origins '*' \
--services b \
--max-age 60 \
--allowed-headers '*' \
--exposed-headers '*' \
--connection-string "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

tener la API en .NET ejecutándose. Más información [aquí](https://github.com/0GiS0/tour-of-heroes-dotnet-api) Sin embargo, para esta versión necesitas que sea el branch **azure-storage-pics**. Este debe tener configurado correctamente la cuenta de almacenamiento que has utilizado en esta parte.

Lo primero que debes hacer es descargarte el proyecto en local y apuntar al branch az-storage-from-js:

```
git clone https://github.com/0GiS0/tour-of-heroes-dotnet-api.git
git checkout az-storage-from-js
```

Instalar las dependencias con npm (si habías ejecutado otros branches anteriormente, procura eliminar la carpeta node_modules si te da problemas):

```
npm install
```

y por último ejecutarlo con start:

```
npm start
```

El proceso arrancará y estará disponible en esta dirección: [http://localhost:4200/](http://localhost:4200/)

## Resultado

El resultado de esta mejora es la siguiente:

![Resultado de az-storage-from-js](images/az-storage-from-js%20resultado.png)
