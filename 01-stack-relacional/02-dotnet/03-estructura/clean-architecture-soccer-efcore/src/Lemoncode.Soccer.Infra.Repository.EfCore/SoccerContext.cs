using Lemoncode.Soccer.Infra.Repository.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lemoncode.Soccer.Infra.Repository.EfCore
{
    public class SoccerContext
        : DbContext
    {
        public SoccerContext(DbContextOptions<SoccerContext> options)
            : base(options)
        {
        }

        //private const string ConnectionString = "Server=localhost;Database=efcore_soccer;user=sa;password=Lem0nCode!";

        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<GoalEntity> Goals { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder
        //        //.UseLazyLoadingProxies()
        //        .UseSqlServer(ConnectionString);

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
    }
}
