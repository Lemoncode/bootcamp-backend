# Introducción a la demo

Para no perder el tiempo con una aplicación que yo haya desarrollado a medida, y que además tengamos todas las partes que necesitamos para entender todo lo que te ofrece Microsoft Azure, he utilizado para el front-end el tutorial de Angular Tour of Heroes, por lo que si quieres puedes seguir los pasos aquí: https://angular.io/tutorial 

![Tour of Heroes](./imagenes/tour-of-heroes.png)

Este termina con una API en memoria como parte de la solución, pero nosotros queremos que sea una API real y que se conecte a una base de datos real. 

Por ello, para el backend, he creado una API en .NET Core, utilizando Entity Framework, y que se apoya en un SQL Server, que devolverá los héroes y nos permitirá manipularlos. Para ello he utilizado este otro tutorial para crear APIs en .NET donde está orientado a una lista de TODOs y yo simplemente lo he cambiado a la lista de héroes que necesitamos: https://docs.microsoft.com/es-es/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code 

El resultado final lo puedes encontrar en la carpeta front-end y back-end de este repositorio.