# crypto-fiat-conversion

Esta librería expone funcionalidad para convertir moneda crypto en moneda fiat.

Para ello utiliza una base de datos interna con el tipo de cambio actual. Esta base de datos interna se actualiza, por ahora, manualmente. En el futuro estará actualizada en todo momento.

## Cómo utilizar
Se instancia `Converter` y se llama al método
```
var result = converter.ConvertToEur("BTC", 5.10);
```
El resultado será un objeto de tipo `ConversionResult` que contiene el código de la criptomoneda, la fecha del cambio actual utilizado, y la cantidad total en EUR para la cantidad especificada.

Es importante tener en cuenta que `Converter` tiene una dependencia a `IPriceDatabase`, y que la instancia de `IPriceDatabase` debe ser única si se utiliza la implementación `InMemoryPriceDatabase`.