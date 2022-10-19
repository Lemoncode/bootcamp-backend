# efcore-soccer

Acceso a SqlServer utilizando EF Core para un modelo E-R de fútbol con equipos, partidos y goles.

Necesita la dependencia `Microsoft.EntityFrameworkCore.SqlServer` para utilizar las librerías de EF Core con SqlServer.
Necesita la dependencia `Microsoft.EntityFrameworkCore.Design` para utilizar migraciones con EF Core CLI.

## Entidades
Hay 3 entidades: `TeamEntity`, `GameEntity` y `GoalEntity` relacionadas entre sí siguiendo el modelo entidad-relación

## DbContext
El `SoccerDbContext` extiende a `DbContext` y contiene los `DbSet` que identifican a cada una de las tablas.

En aplicaciones consola, el `DbContext` debe tener un método `OnConfiguring(DbContextOptionsBuilder options)` donde se le configura el provider (i.e: SqlServer)

## Instalar EF Core CLI
Instala el CLI de Entity Framework Core con
```
dotnet tool install --global dotnet-ef
```
o con
```
dotnet tool update --global dotnet-ef
```
para tener la última versión.

## Migraciones
Para crear una migración, o la inicial cuando se desea crear la base de datos, ejecuta desde el directorio raíz de la solución:
```
dotnet ef migrations add inicial --project src/LemonCode.EfCore.Soccer
```

Un directorio llamado `Migrations` será automáticamente creado.

Para aplicar las migraciones que hayan sido generadas, ejecuta:
```
dotnet ef database update --project src/LemonCode.EfCore.Soccer
```

## Notas
Por la naturaleza de este modelo entidad relación, en concreto, en el que un `Game` está asociado a dos equipos, habrá un error al aplicar las migraciones
```
Microsoft.Data.SqlClient.SqlException (0x80131904): Introducing FOREIGN KEY constraint 'FK_Games_Teams_HomeTeamId' on table 'Games' may cause cycles or multiple cascade paths. 
Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints
```

Este error se puede solucionar haciendo explícitas esas relaciones.

```
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder
        .Entity<GameEntity>()
        .HasOne<TeamEntity>(x => x.HomeTeam)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder
        .Entity<GameEntity>()
        .HasOne<TeamEntity>(x => x.AwayTeam)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

}
```