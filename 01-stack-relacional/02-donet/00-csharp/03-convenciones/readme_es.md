# CONVENCIONES

Las convenciones en el código nos ayudan a:

- Tener un código consistente, con lo que un desarrollador que lea el código se pueda centrar en el contenido del código.
- Ayuda al desarrollador a entender de manera más fácil y rápida el código ya que todos seguimos las mismas convenciones.
- Facilita copiar, cambiar y mantener el código.
- En general, son buenas prácticas que todos hemos de seguir

## CONVENCIONES DE NOMENCLATURA

En *.Net* como en cualquier otro lenguaje existen unas normas básicas para el uso de mayúsculas y minúsculas.

Dependiendo del identificador podemos distinguir dos nomenclaturas:

1. **Pascal Case**: se usa para la mayoría de identificadores:

    - Espacios de nombres:  `namespace Vehiculos.Contratos`

    - Clases:  `public class Coche {}`

    - Interfaces: `public interface IVehiculo{}`

    - Propiedades: `public int NumeroRuedas {get; set;}`

    - Métodos: `public int ObtenerNumeroRuedas(){}`

    - Valor de enumerados:

    ```csharp
    public enum Marca {
        Mercedes,
        Audi,
        Ford,
        Seat
    }
    ```

2. **camel Case**: se utiliza para variables y parámetros

    ```csharp
    public class Coche {
        public int ObtenerNumeroRuedas(Marca marcaVehiculo){
            int numeroRuedas = 0;
            ...
        }
    }
    ```

## CONVENCIONES GENERALES

1. Los nombres de los identificadores han de ser fáciles de leer
`NumeroRuedas` es más legible que `RuedasNumero`

2. No utilizar abreviaturas para mejorar la legibilidad. Por ejemplo, el método `ObtenerNumeroRuedas` es más legible que si se llamara simplemente `Ruedas`

3. Evitar el uso de acrónimos

4. Escribe sólo una sentencia o declaración por línea

5. Para los comentarios, este ha de ir en una línea a parte.

6. Empieza el comentario con mayúsculas.

7. Deja un espacio en blanco entre el delimitador de comentario // y el texto

```csharp
// Esto es un comentario
```
