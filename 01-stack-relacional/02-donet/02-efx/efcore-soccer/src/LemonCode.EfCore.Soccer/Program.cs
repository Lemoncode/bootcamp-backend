using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LemonCode.EfCore.Soccer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LemonCode.EfCore.Soccer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await RunExample();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine("\nPress [ENTER] to end");
            Console.ReadLine();
        }

        private static async Task RunExample()
        {
            await RunInserts();
            await RunQueries();
        }

        private static async Task RunInserts()
        {
            var realMadrid = new TeamEntity { Code = "RMA" };
            var barcelona = new TeamEntity { Code = "BAR" };
            var malaga = new TeamEntity { Code = "MGA" };
            var atletico = new TeamEntity { Code = "ATM" };

            var teams =
                new List<TeamEntity>
                {
                    realMadrid,
                    barcelona,
                    malaga,
                    atletico,
                    new() { Code = "ALV" },
                    new() { Code = "PAL" },
                    new() { Code = "SEV" },
                    new() { Code = "DEP" },
                    new() { Code = "CEL" },
                    new() { Code = "VAD" },
                    new() { Code = "GRA" },
                    new() { Code = "OSA" },
                    new() { Code = "RSO" },
                    new() { Code = "RAC" },
                    new() { Code = "COR" },
                    new() { Code = "EIB" },
                    new() { Code = "RAY" },
                    new() { Code = "VAL" },
                    new() { Code = "VIL" },
                    new() { Code = "ESP" }
                };

            var games =
                new List<GameEntity>
                {
                    new()
                    {
                        HomeTeam = teams.Single(x => x.Code == "RMA"),
                        AwayTeam = teams.Single(x => x.Code == "BAR"),
                        ScheduledOn = new DateTime(2021, 10, 27, 18, 0, 0),
                        StartedOn = new DateTime(2021, 10, 27, 18, 0, 10),
                        EndedOn = null,
                        Goals =
                            new List<GoalEntity>
                            {
                                new()
                                {
                                    ScoredBy = "Karim Benzema",
                                    Team = realMadrid,
                                    ScoredOn = new DateTime(2021, 10, 27, 18, 5, 25)
                                },
                                new()
                                {
                                    ScoredBy = "Vinicius Jr",
                                    Team = realMadrid,
                                    ScoredOn = new DateTime(2021, 10, 27, 18, 5, 25)
                                }
                            }
                    },
                    new()
                    {
                        HomeTeam = teams.Single(x => x.Code == "MGA"),
                        AwayTeam = teams.Single(x => x.Code == "ATM"),
                        ScheduledOn = new DateTime(2021, 10, 27, 18, 0, 0),
                        StartedOn = new DateTime(2021, 10, 27, 18, 0, 10),
                        EndedOn = null,
                        Goals =
                            new List<GoalEntity>
                            {
                                new()
                                {
                                    ScoredBy = "Antonio Cortés",
                                    Team = malaga,
                                    ScoredOn = new DateTime(2021, 10, 27, 18, 7, 15)
                                },
                                new()
                                {
                                    ScoredBy = "Luis Suarez",
                                    Team = atletico,
                                    ScoredOn = new DateTime(2021, 10, 27, 18, 10, 00)
                                }
                            }
                    }
                };

            await using var soccerContext = new SoccerContext();
            await soccerContext.Games.AddRangeAsync(games);
            // Implicitly, goals and teams are added
            await soccerContext.SaveChangesAsync();
            Console.WriteLine("Data inserted");

            await soccerContext.DisposeAsync(); // We can dispose the soccer context and re-create
        }

        private static async Task RunQueries()
        {
            await using var soccerContext = new SoccerContext();

            // Get teams
            var teams = await soccerContext.Teams.ToListAsync();
            Console.WriteLine("Teams:");
            foreach (var team in teams)
            {
                Console.WriteLine(team.Code);
            }

            // Get teams filtered in DB
            var teamsFiltered = await soccerContext.Teams.Where(x => x.Code.Contains("M")).ToListAsync();
            Console.WriteLine("\nTeams containing 'M':");
            foreach (var team in teamsFiltered)
            {
                Console.WriteLine(team.Code);
            }

            // Get teams filtered in memory
            var teamsFilteredInMemory = (await soccerContext.Teams.ToListAsync()).Where(x => x.Code.Contains("M"));
            Console.WriteLine("\nTeams containing 'M':");
            foreach (var team in teamsFilteredInMemory)
            {
                Console.WriteLine(team.Code);
            }

            // Get teams query without hitting database
            var teamsQuery = soccerContext.Teams.Where(x => x.Code.Contains("M")).AsQueryable();
            var teamsQueryWithAdditionalFilter = teamsQuery.Where(x => x.Code.Contains("A")).AsQueryable();
            var teamsQueryWithAdditionalFilterAndPagination = teamsQueryWithAdditionalFilter.Skip(1).Take(10).AsQueryable();
            var sqlQuery = teamsQueryWithAdditionalFilterAndPagination.ToQueryString();
            Console.WriteLine($"Query without hitting database: {sqlQuery}");

            // Ejecutar consulta
            var teamsDelayedExecution = await teamsQueryWithAdditionalFilterAndPagination.ToListAsync();
            Console.WriteLine("\nTeams delayed execution, skipping 1:");
            foreach (var team in teamsDelayedExecution)
            {
                Console.WriteLine(team.Code);
            }


            // Attempt to view goals when querying teams
            var teamsWithGoalsAttempted = await soccerContext.Teams.ToListAsync();
            Console.WriteLine("Teams (with goals?):");
            foreach (var team in teamsWithGoalsAttempted)
            {
                Console.WriteLine(team.Code);
                var goals = team.Goals;
                foreach (var goal in goals)
                {
                    Console.WriteLine($"{goal.ScoredOn} - {goal.ScoredBy}");
                }
            }

            // Eager loading
            var teamsWithGoalsEagerLoading = await soccerContext.Teams.Include(x => x.Goals).ToListAsync();
            Console.WriteLine("\nTeams with goals (eager loading):");
            foreach (var team in teamsWithGoalsEagerLoading)
            {
                Console.WriteLine(team.Code);
                var goals = team.Goals;
                foreach (var goal in goals)
                {
                    Console.WriteLine($"{goal.ScoredOn} - {goal.ScoredBy}");
                }
            }

            // Lazy loading (tras añadir UseLazyLoadingProxies() y hacer las propiedades de navegación virtuales)
            var teamsWithGoalsLazyLoading = await soccerContext.Teams.ToListAsync();
            Console.WriteLine("\nTeams with goals (lazy loading):");
            foreach (var team in teamsWithGoalsLazyLoading)
            {
                Console.WriteLine(team.Code);
                var goals = team.Goals;
                foreach (var goal in goals)
                {
                    Console.WriteLine($"{goal.ScoredOn} - {goal.ScoredBy}");
                }
            }
        }
    }
}
