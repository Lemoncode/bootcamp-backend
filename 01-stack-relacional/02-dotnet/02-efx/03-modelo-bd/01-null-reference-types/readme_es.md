# Notas importantes sobre Null Reference Types (NRT)

Desde la versión 8.0 de C#, existe una característica muy interesante llamada NRT, o _null reference type_. En nuevos proyectos (desde .Net 5), cualquier tipo por referencia que normalmente admitiría null, ya no lo admitirá si no se lo ponemos de forma explícita. Vamos a verlo con un ejemplo.

Estamos en el proyecto _LibraryManagerWeb_. Vamos a pulsar un doble clic sobre el proyecto y se nos abrirá el editor xml para archivos csproj.
 
 Si miramos en la parte superior del csproj, veremos que esta característica ya está activada por defecto.

```xml
<PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <InvariantGlobalization>false</InvariantGlobalization>
</PropertyGroup>
``````

Lo que tenemos que tener claro es que por defecto en nuevos proyectos, cualquier tipo por referencia o de tipo string (que son inmutables y se manejan internamente como referencias aunque externamente no lo parezca), ahora no aceptarán nulls por defecto, y esto deberemos tenerlo en cuenta desde la concepción de nuestro modelo. Si queremos que un valor por referencia admita nulls, deberemos indicárselo con el operador de nulabilidad (nullable operator) _?_.

También, desde c# 11.0, existe un nuevo modificador para propiedades, llamado _required_. Este modificador exigirá al compilador que los objetos que tengan propiedades _required_, tengan que ser instanciados con estas propiedades inicializadas. Así, si queremos marcar una propiedad como obligatoria, el modificador _required_ es una excelente manera de hacerlo, ya que se eliminará el warning de VS Studio que nos dice que una propiedad por referencia puede estar inicializada a null al salir del constructor (_CS8618. Non-nullable property 'Sinopsis' must contain a non-null value when exiting constructor. Consider declaring the property as nullable._), y también el código es mucho más claro, pues al leerlo , vamos a entender que esa propiedad es obligatoria.

En resunmen, para propiedades por referencia:

- Marcar con el operador de nulabilidad (_?) aquellas propiedades opcionales.
- Marcar con el modificador _required_ aquellas propiedades obligatorias.

Esto, por supuesto, sin prejuicio de poder utilizar las anotaciones de datos o la API fluida que veremos más adelante, para forzar a EF a considerar propiedades como obligatorias o no.