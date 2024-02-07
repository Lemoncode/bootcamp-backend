[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/0gis0/tour-of-heroes-angular)

# Aplicación de ejemplo en Angular: Tour Of Heroes

Este proyecto es una aplicación en AngularJS que muestra un listado de heroes. Proviene del [tutorial de AngularJS](https://angular.io/tutorial). Sin embargo, se han llevado a cabo ciertas modificaciones para que este utilice una API en .NET Core que puedes encontrar [aquí](https://github.com/0GiS0/tour-of-heroes-dotnet-api).

Para ello, se ha modificado el archivo **app/app.module.ts** para comentar los archivos que referencian a la API en memoria del tutorial.

```
//In memory web api
// import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
// import { InMemoryDataService } from './in-memory-data.service';
```

Y el que se referencia en el apartado imports:

```
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    // HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, { dataEncapsulation: false })
  ],
```

Por otro lado, se ha modificado el archivo **app/hero.service.ts** para utilizar una variable que referencie a la API real:

```
export class HeroService {

  // private heroesUrl = 'api/heroes';
  private heroesUrl = environment.apiUrl; //URL to the web api
```

Los valores de enviroment se encuentran en los archivos **src/environments/enviroment.ts** y **src/environments/environment.prod.ts**. Dependiendo de cómo se compile el proyecto se utilizará uno u otro (environment.ts para desarrollo y environment.prod.ts para producción).

y un cambio mínimo en el método update:

```
  /** PUT: update the hero on the server */
  updateHero(hero: Hero): Observable<any> {

    // Create the route - getting 405 Method not allowed errors
    const url = `${this.heroesUrl}/${hero.id}`;

    return this.http.put(url, hero, this.httpOptions).pipe(
      tap(_ => this.log(`updated hero id=${hero.id}`)),
      catchError(this.handleError<any>('updateHero'))
    );
  }
```

## Cómo lo ejecuto

**IMPORTANTE**: Antes de ejecutar este proyecto necesitas tener la API en .NET ejecutándose. Más información [aquí](https://github.com/0GiS0/tour-of-heroes-dotnet-api)

Lo primero que debes hacer es descargarte el proyecto en local:

```
git clone https://github.com/0GiS0/tour-of-heroes-dotnet-api.git
```

Instalar las dependencias con npm:

```
npm install
```

y por último ejecutarlo con start:

```
npm start
```

El proceso arrancará y estará disponible en esta dirección: [http://localhost:4200/](http://localhost:4200/)
