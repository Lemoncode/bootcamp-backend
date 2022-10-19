# hash-function

Función hash para securizar contraseñas en forma de extensión de string con tests unitarios.

## Método de extensión
En C# se puede extender cualquier tipo de datos con nuevos métodos sin necesidad de utilizar herencia, recompilar o modificar el tipo original.

Para ello, dentro de una clase estática, crea un método estático que acepte como parámetro `this <type_to_extend> value`

Este ejemplo demuestra cómo crear una extensión de strings para obtener su hash con SHA256, por el cual los hash de dos valores coinciden si solo si los valores son iguales.s