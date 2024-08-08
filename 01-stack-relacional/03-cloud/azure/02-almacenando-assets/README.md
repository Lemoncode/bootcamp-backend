# Almacenando assets en Microsoft Azure

En la clase anterior ya viste lo sencillo que era desplegar nuestro Tour Of Heroes en Microsoft Azure, usando SQL Azure, para la base de datos, App Service, para la API en .NET, y Azure Static Web Apps, para el frontal en Angular. En esta clase vamos a ver cómo almacenar los assets de nuestra aplicación en el servicio Azure Storage.

## ¿Qué es un asset?

Un asset es cualquier recurso que necesite ser almacenado y servido a través de la web. Puede ser una imagen, un archivo de audio, un video, un archivo de texto, un archivo de configuración, etc.

En Microsoft Azure tenemos justamente lo que necesitamos y se llama Azure Storage. Azure Storage es un servicio de almacenamiento en la nube que es seguro, escalable, duradero y altamente disponible. Puedes almacenar cualquier tipo de archivo y acceder a él desde cualquier lugar del mundo.

**El objetivo de la clase de hoy será que nuestra aplicación Angular se modernice un poco para que nuestro héroes tengan cara y capa, además de poder ver su identidad secreta**. Para ello, vamos a almacenar las imágenes de nuestros héroes en Azure Storage. Pero antes, vamos a probar la aplicación en local con los cambios:

En primer lugar asegurate que la base de datos, que teníamos ejecutándose en Docker, sigue en marcha. Si no es así, ejecuta el siguiente comando:

```bash
docker start sqlserver
```

Ejecuta los siguientes comandos para cargar la API:

```bash
cd 01-stack-relacional/03-cloud/azure/02-almacenando-assets/back-end
dotnet run
```

entra en el directorio `front-end` de esta clase y ejecuta los siguientes comando para cargar la interfaz:

```bash
cd 01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end
npm install
npm start
```

Como puedes ver, ahora los héroes tienen una imagen de perfil. 

<img src="docs-img/Tour of heroes con imágenes.png">

Si haces clic en el nombre del héroe, podrás ver su identidad secreta. Ahora vamos a subir estas imágenes a Azure Storage.

<img src="docs-img/Identidad secreta en tour of heroes.png">

Estas imágenes a día de hoy forman parte del código de la aplicación, pero lo ideal es que estuvieran en un lugar independiente, como Azure Storage, para que puedan ser actualizadas sin necesidad de desplegar la aplicación, además de que hará que mejorar el rendimiento de la aplicación.

## Creando una cuenta de Azure Storage

Antes de crear la cuenta de Azure Storage vamos a recuperar la variable que creamos en la clase anterior con el grupo de recursos. Abre una terminal y ejecuta el siguiente comando:

```bash
RESOURCE_GROUP="tour-of-heroes"
```

También vamos a cargar una variable con el nombre de la cuenta de Azure Storage que vamos a crear y la localización:

```bash
STORAGE_ACCOUNT="heroespics"
LOCATION="spaincentral"
```

Ahora vamos a crear la cuenta de Azure Storage. Abre una terminal y ejecuta el siguiente comando:

```bash
az storage account create \
--resource-group $RESOURCE_GROUP \
--name $STORAGE_ACCOUNT \
--location $LOCATION \
--sku Standard_LRS \
--allow-blob-public-access
```

>Nota: a día de hoy hay que utilizar la opción `--allow-blob-public-access` para que nos permita tener contenedores con acceso público, porque de lo contrario no nos dejará crearlos.

Una vez que tenemos la cuenta creada el siguiente paso es crear dos contendores, uno para las imágenes de los héroes y otro para las imágenes de las identidades secretas o alter egos. Estos contenedores nos permiten organizar los archivos que almacenamos en Azure Storage, además de gestionar los permisos de acceso a los mismos.

```bash
az storage container create \
--name heroes \
--account-name $STORAGE_ACCOUNT \
--public-access blob
```

Como puedes ver, este contenedor se llama `heroes` y tiene acceso público. Esto significa que cualquier persona que conozca la URL de una imagen podrá acceder a ella. En el caso de las imágenes de los héroes, esto es lo que queremos, ya que queremos que todos sepan cómo son nuestros héroes.

Ahora vamos a crear un segundo contenedor llamado `alteregos` para las imágenes de las identidades secretas, el cual debe ser privado:

```bash
az storage container create \
--name alteregos \
--account-name $STORAGE_ACCOUNT
```

En este caso el contenedor `alteregos` es privado, lo que significa que solo las personas que tengan una URL firmada podrán acceder a las imágenes. Esto es lo que queremos para las identidades secretas de nuestros héroes.

## Subiendo las imágenes a Azure Storage

Para subir las imágenes a Azure Storage tenemos varias formas: 

- A través del portal de Azure.
- Usando Microsoft Azure Storage Explorer.
- Usando la interfaz de línea de comandos de Azure.
- Usando AzCopy
- Alguna librería de cliente para Azure Storage.
- A través de la API REST de Azure Storage.

Para hacerlo a través de la línea de comandos puedes utilizar el siguiente comando:

```bash
az storage blob upload-batch \
--destination alteregos \
--source 01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end/src/assets/alteregos/. \
--account-name $STORAGE_ACCOUNT
```

Como ves, con tan solo un comando hemos subido todos los alter egos de nuestros héroes a Azure Storage. Ahora vamos a subir las imágenes de los héroes:

```bash
az storage blob upload-batch \
--destination heroes \
--source 01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end/src/assets/heroes/. \
--account-name $STORAGE_ACCOUNT
```

Si ahora vas al portal, o a Azure Storage Explorer, podrás comprobar que las imágenes están subidas a tu nueva cuenta en el contenedor correspondiente.

## Actualizando la aplicación para que use las imágenes de Azure Storage

Si bien es cierto que nuestras imágenes están ya en la nube, nuestra aplicación sigue haciendo uso de la copia que tiene en local. Vamos a cambiar esto para que nuestra aplicación use las imágenes de Azure Storage.

Para ello ve al archivo `01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end/src/app/heroes/heroes.component.html`y modificalo de la siguiente manera:

```html
<h2>My heroes</h2>
<div>
  <label for="new-hero">Hero name: </label>
  <input id="new-hero" #heroName />
  <button class="add-button" (click)="add(heroName.value); heroName.value = ''">
    Add hero
  </button>
</div>

<div id="features-wrapper">
  <div class="container">
    <div class="row">
      <div class="col-4 col-12-medium" *ngFor="let hero of heroes">
        <!-- Box -->
        <section class="box feature">
          <!-- <a routerLink="/detail/{{ hero.id }}" class="image featured"
            ><img
              src="assets/heroes/{{
                hero.name | lowercase | replace: ' ' : '-'
              }}.jpeg"
              alt=""
          /></a> -->
          <a routerLink="/detail/{{hero.id}}" class="image featured">
            <img
              src="https://heroespics.blob.core.windows.net/heroes/{{hero.name | lowercase | replace: ' ':'-'}}.jpeg"
              alt="" />
          </a>
          <div class="inner">
            <header>
              <h2>{{ hero.name }}</h2>
            </header>
            <p>{{ hero.description | slice: 0 : 150 }}...</p>
            <button (click)="delete(hero)">Delete</button>
          </div>
        </section>
      </div>
    </div>
  </div>
</div>
```

Lo único que he hecho ha sido comentar la línea que hace referencia a la imagen local y añadir una nueva línea que hace referencia a la imagen de Azure Storage. ¿Fácil verdad? 😉

Ahora vamos a hacer lo mismo con el archivo `01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end/src/app/hero-detail/hero-detail.component.html`:

```html
<div id="features-wrapper">
  <div class="container">
    <div class="row" *ngIf="hero">
      <div class="col-5 col-12-medium">
        <!-- Box -->
        <section class="box feature">
          <!-- <a routerLink="/detail/{{ hero.id }}" class="image featured"
            ><img
              src="assets/alteregos/{{
                hero.alterEgo | lowercase | replace: ' ' : '-'
              }}.png"
              alt=""
          /></a> -->
          <a routerLink="/detail/{{hero.id}}" class="image featured">
            <img src="https://heroespic.blob.core.windows.net/alteregos/{{hero.alterEgo | lowercase | replace: ' ':'-'}}.png" alt="" />
          </a>
        </section>
      </div>
      <div class="col-7">
        <form>
          <div class="form-group">
            <label for="hero-name">
              <input
                id="hero-name"
                [(ngModel)]="hero.name"
                [ngModelOptions]="{ standalone: true }"
                placeholder="Name"
              />
              <span>Hero name</span>
            </label>
          </div>
          <div class="form-group">
            <label for="hero-name">
              <input
                id="hero-name"
                [(ngModel)]="hero.alterEgo"
                [ngModelOptions]="{ standalone: true }"
                placeholder="Alter ego"
              />
              <span>Alter ego</span>
            </label>
          </div>
          <div class="form-group">
            <label for="hero-name">
              <textarea
                id="hero-description"
                [(ngModel)]="hero.description"
                [ngModelOptions]="{ standalone: true }"
                placeholder="Description"
              ></textarea>
              <span>Description</span>
            </label>
          </div>
          <div class="buttons">
            <button (click)="save()">Save</button>
            <button (click)="goBack()">Go back</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</div>
```

Pero... Ops! 🧐 en este caso no funciona ¿Por qué? Si abrimos las **Developer Tools** vemos que tenemos un error.

<img src="docs-img/Error con el contenedor privado.png" />

Pero ¿cómo qué no existe? Si acabamos de subir las imágenes. Pues bien, el problema es que el contenedor `alteregos` es privado, por lo que necesitamos que alguien con permisos las recupere por vosotros. La mejor forma de hacer eso es modificar nuestra API para que nos devuelva estos alter egos.

## Devolviendo los alter egos a través de la API

En nuestra carpeta `back-end` tienes que descomentar el método `GetAlterEgoPic`el cuál se encarga de devolver la URL firmada de la imagen del alter ego. El método debería quedar de la siguiente manera:

```csharp
        // GET: api/hero/alteregopic/5
        [HttpGet("alteregopic/{id}")]
        public async Task<ActionResult<Hero>> GetAlterEgoPic(int id)
        {
            var hero = await _context.Heroes.FirstOrDefaultAsync(h => h.Id == id);

            if (hero == null)
            {
                return NotFound();
            }

            //Get image from Azure Storage
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            
            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);

            //Get container client
            var containerClient = blobServiceClient.GetBlobContainerClient("alteregos");

            //Get blob client
            var blob = containerClient.GetBlobClient($"{hero.AlterEgo.ToLower().Replace(' ', '-')}.png");

            //Get image from blob
            var image = await blob.DownloadStreamingAsync();

            //return image
            return File(image.Value.Content, "image/png");
        }
```

Este como ves es un método muy sencillo que se encarga de devolver la imagen del alter ego. Para que esto funcione he tenido que agregar el paquete de nuget  `Azure.Storage.Blobs` en el proyecto. 

```bash
dotnet add package Azure.Storage.Blobs 
```

Y añadir el using correspondiente en el archivo `Controllers/HeroController.cs`:

```csharp
using Azure.Storage.Blobs;
```

Ahora vamos a guardar en una variable la cadena de conexión necesaria para que esta pueda comunicarse con Azure Storage. En el terminal de la API lanza lo siguiente:

```bash
STORAGE_ACCOUNT="heroespics2"
RESOURCE_GROUP="tour-of-heroes-2"

CONNECTION_STRING=$(az storage account show-connection-string \
--name $STORAGE_ACCOUNT \
--resource-group $RESOURCE_GROUP \
--query connectionString \
--output tsv)
```

Ahora que ya la tienes vamos a setear la variable de entorno que nuestra API necesita para poder comunicarse con Azure Storage.

```bash
AZURE_STORAGE_CONNECTION_STRING=$CONNECTION_STRING dotnet run
```

Y voilá! Ahora si que si, si vamos a la URL `https://localhost:5001/api/hero/alteregopic/2` deberíamos ver la imagen del alter ego de nuestro héroe.

Ahora solo queda que nuestro frontal en Angular sepa llamar a esta nueva acción de nuestra API. Para ello añade en `01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end/src/app/hero.service.ts`el siguiente método:

```typescript
  getAlterEgoPic(id: number): Observable<Blob> {
    return this.http.get(`${this.heroesUrl}/alteregopic/${id}`, { responseType: 'blob' });
  }
```

y modifica `01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end/src/app/hero-detail/hero-detail.component.ts` para que llame a este nuevo método:

```typescript
import { Component, OnInit, Input } from '@angular/core';
import { Hero } from '../hero';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { HeroService } from '../hero.service';

@Component({
  selector: 'app-hero-detail',
  templateUrl: './hero-detail.component.html',
  styleUrls: ['./hero-detail.component.css']
})
export class HeroDetailComponent implements OnInit {

  @Input() hero?: Hero;
  alterEgoPic?: any;

  constructor(private route: ActivatedRoute, private heroService: HeroService, private location: Location) { }

  ngOnInit(): void {
    this.getHero();
  }

  getHero(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.heroService.getHero(id).subscribe(hero => this.hero = hero);
    
    this.heroService.getAlterEgoPic(id).subscribe(alterEgoPic => {
      let reader = new FileReader();
      reader.onload = (e: any) => {
        this.alterEgoPic = e.target.result;
      };
      
      if (alterEgoPic){
        reader.readAsDataURL(alterEgoPic);
      }
      
    });
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    if (this.hero) {
      this.heroService.updateHero(this.hero)
        .subscribe(() => this.goBack());
    }
  }

}
```

Y ya por último modifica `01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end/src/app/hero-detail/hero-detail.component.html` para que muestre la imagen del alter ego:

```html
<div id="features-wrapper">
  <div class="container">
    <div class="row" *ngIf="hero">
      <div class="col-5 col-12-medium">
        <!-- Box -->
        <section class="box feature">
          <!-- <a routerLink="/detail/{{ hero.id }}" class="image featured"
            ><img
              src="assets/alteregos/{{
                hero.alterEgo | lowercase | replace: ' ' : '-'
              }}.png"
              alt=""
          /></a> -->
          <!-- <a routerLink="/detail/{{hero.id}}" class="image featured">
            <img src="https://heroespics.blob.core.windows.net/alteregos/{{hero.alterEgo | lowercase | replace: ' ':'-'}}.png" alt="" />
          </a> -->
          <a routerLink="/detail/{{hero.id}}" class="image featured"><img src="{{alterEgoPic}}" alt="" /></a>
        </section>
      </div>
      <div class="col-7">
        <form>
          <div class="form-group">
            <label for="hero-name">
              <input
                id="hero-name"
                [(ngModel)]="hero.name"
                [ngModelOptions]="{ standalone: true }"
                placeholder="Name"
              />
              <span>Hero name</span>
            </label>
          </div>
          <div class="form-group">
            <label for="hero-name">
              <input
                id="hero-name"
                [(ngModel)]="hero.alterEgo"
                [ngModelOptions]="{ standalone: true }"
                placeholder="Alter ego"
              />
              <span>Alter ego</span>
            </label>
          </div>
          <div class="form-group">
            <label for="hero-name">
              <textarea
                id="hero-description"
                [(ngModel)]="hero.description"
                [ngModelOptions]="{ standalone: true }"
                placeholder="Description"
              ></textarea>
              <span>Description</span>
            </label>
          </div>
          <div class="buttons">
            <button (click)="save()">Save</button>
            <button (click)="goBack()">Go back</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</div>
```

## Desplegar los cambios en Azure

Ahora que ya tenemos nuestra nueva funcionalidad lista, vamos a desplegar tanto los cambios de la API como del frontal. 

### Desplegar el frontal

Si hiciste un fork del repo de Lemoncode tienes dos opciones:

En el caso del frontal es muy sencillo, si en la clase anterior me hiciste caso 😃 e hiciste un fork de este repositorio. Ya que simplemente sincronizando los cambios que acabo de mostrarte solo tienes que darle al botón de **Sync Fork** en el branch que corresponda y automáticamente se desplegarán los cambios.

<img src="docs-img/Sincronizar el fork.png" />

Sin embargo, existe otra opción más chula que sería crear un nuevo branch en tu fork y descargarse en él los cambios que acabo de mostrarte. 


```bash
git checkout -b almacenando-assets
git remote add upstream https://github.com/Lemoncode/bootcamp-backend
git config pull.rebase false
git pull upstream gisela/azure
git push origin almacenando-assets
```

¿Y con esto qué conseguimos? Pues que podamos probar de forma sencilla los entornos paralelos que Azure Static Web Apps nos ofrece.

Lo único que nos queda por hacer es ir a nuestro repositorio en GitHub y crear un nuevo pull request con este nuevo branch.

<img src="docs-img/Crear una PR con este nuevo branch.png" />

La configuración sería esta:

<img src="docs-img/La configuración para la PR.png" />

Esto lanzará nuestros flujos de GitHub Actions y generará un nuevo entorno en Azure Static Web Apps con los cambios de hoy 😎.

<img src="docs-img/Nuevo entorno en la Static Web App con el cambio de interfaz.png" />

En este caso, lamentablemente, no funcionará como esperamos porque en el caso de este repo, al cambiar la ruta de dónde se encuentra la aplicación, no sabe que ahora tiene que usar el path `01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end` para la propiedad `app_location` del workflow de GitHub Actions y cuando se crea la pull request sigue usando el directorio de la clase anterior, por lo que no podemos ver los cambios en el entorno de preproducción. Pero si me parecía chulo que vieras cómo se hace 😃.

Por que igualmente, podemos ahora si, sincronizar los cambios del fork y modificar nuestro workflow de GitHub Actions para que despliegue nuestra nueva funcionalidad en producción:

```yaml
name: Azure Static Web Apps CI/CD

on:
  push:
    branches:
      - gisela/azure
  pull_request:
    types: [opened, synchronize, reopened, closed]
    branches:
      - gisela/azure

jobs:
  build_and_deploy_job:
    if: github.event_name == 'push' || (github.event_name == 'pull_request' && github.event.action != 'closed')
    runs-on: ubuntu-latest
    name: Build and Deploy Job
    steps:
      - uses: actions/checkout@v3
        with:
          submodules: true
          lfs: false
      - name: Build And Deploy
        id: builddeploy
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN_GREEN_SEA_060476103 }}
          repo_token: ${{ secrets.GITHUB_TOKEN }} # Used for Github integrations (i.e. PR comments)
          action: "upload"
          ###### Repository/Build Configurations - These values can be configured to match your app requirements. ######
          # For more information regarding Static Web App workflow configurations, please visit: https://aka.ms/swaworkflowconfig
          app_location: "01-stack-relacional/03-cloud/azure/02-almacenando-assets/front-end" # App source code path
          api_location: "" # Api source code path - optional
          output_location: "dist/angular-tour-of-heroes" # Built app content directory - optional
          ###### End of Repository/Build Configurations ######

  close_pull_request_job:
    if: github.event_name == 'pull_request' && github.event.action == 'closed'
    runs-on: ubuntu-latest
    name: Close Pull Request Job
    steps:
      - name: Close Pull Request
        id: closepullrequest
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN_GREEN_SEA_060476103 }}
          action: "close"
```

>Nota: No copies y pegues este código en tu repositorio, ya que el token que se usa aquí es el mío y no te funcionará. Solo copia el valor de la propiedad `app_location` y pégalo en tu workflow.

### Desplegar los cambios de la API

Y para esta ocasión, para desplegar los cambios de la API, vamos a usar otra de las funcionalidades que App Service nos ofrece, que son los slots. Por lo que primero vamos a crear un slot para nuestra API.

```bash
BACK_END_NAME=tour-of-heroes-api-2

az webapp deployment slot create \
--name $BACK_END_NAME \
--resource-group $RESOURCE_GROUP \
--slot staging
```

>Nota: Lamentablemente esta funcionalidad no está disponible para el plan gratuito, por lo que si estás usando el plan gratuito, no podrás hacer este ejercicio. Si quisieras probarlo, debemos cambiar el plan de la API a uno de pago.

```bash
az appservice plan update \
--name $BACK_END_NAME \
--resource-group $RESOURCE_GROUP \
--sku S1
```

Y ahora ya si intentar crear el slot:

```bash
az webapp deployment slot create \
--name $BACK_END_NAME \
--resource-group $RESOURCE_GROUP \
--slot staging \
--configuration-source $BACK_END_NAME
```

Además, vamos a utilizar la extensión de Visual Studio Code llamada **Azure App Service** para poder ver nuestro servicio. Una vez instalada, si accedemos en el menú lateral izquiero al icono de Azure podrás ver tus servicios de Azure.

Ahora, para poder desplegar nuestra API en el slot que acabamos de crear, vamos a publicar el código como hicimos en la clase anterior y generar el zip:

```bash
cd 01-stack-relacional/03-cloud/azure/02-almacenando-assets/back-end
dotnet publish -o ./publish

cd publish
zip -r site.zip *

az webapp deployment source config-zip \
--src site.zip \
--resource-group $RESOURCE_GROUP \
--name $BACK_END_NAME \
--slot staging
```

A través de la extensión podrás ver que se ha creado un nuevo slot y que está desplegada la nueva versión de la API en él.

Para probarla puedes usar la URL que te proporciona la extensión o bien acceder a la URL de la API y añadirle el nombre del slot que acabas de crear. En mi caso sería `https://tour-of-heroes-api-staging.azurewebsites.net/api/hero/alteregopic/2`.

Ops! Parece que no funciona, ¿verdad? 🧐 Esto es porque hemos copiado la configuración que teníamos la API del entorno de producción donde solo dependíamos de la base de datos. Pero es que ahora también dependemos de nuestra cuenta de Azure Storage. Por lo que es necesario añadirle el valor que en local le pasamos como variable de entorno como un App Setting de nuestro slot de App Service:

```bash
az webapp config appsettings set \
--name $BACK_END_NAME \
--resource-group $RESOURCE_GROUP \
--slot staging \
--settings "AZURE_STORAGE_CONNECTION_STRING=$CONNECTION_STRING"
```

Y ahora si, si volvemos a probar la URL, veremos que funciona correctamente.

Pero espera, el frontal no sabe que ahora tiene que usar el slot de staging para la API. Por lo que vamos a modificar el archivo `environment.prod.ts` para que use el nuevo endpoint:

```typescript
export const environment = {
  production: true,
  apiEndpoint: 'https://tour-of-heroes-api-staging.azurewebsites.net/hero/api'
};
```

Guarda los cambios y vuelve a probar la web una vez que el flujo de GitHub Actions haya terminado.