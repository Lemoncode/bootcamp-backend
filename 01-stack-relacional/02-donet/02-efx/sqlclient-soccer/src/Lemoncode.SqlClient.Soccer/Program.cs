using System;
using System.Threading.Tasks;

namespace Lemoncode.SqlClient.Soccer
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

            Console.WriteLine("Press [ENTER] to end");
            Console.ReadLine();
        }

        private static async Task RunExample()
        {
            const string connectionString = "Data Source=localhost;User Id=sa;Password=Lem0nCode!;";
            const string databaseName = "sqlclient_soccer";
            var soccerClient = new SoccerClient(connectionString, databaseName);
            await soccerClient.Connect();

            await soccerClient.DeleteDatabase();
            await soccerClient.CreateDatabase();
            Console.WriteLine($"Database {databaseName} re-created");

            await soccerClient.CreateTables();
            Console.WriteLine("Tables created");

            await soccerClient.InsertData();
            Console.WriteLine("Data inserted");

            await soccerClient.QueryData(Console.WriteLine); // Same as x => Console.WriteLine(x)

            await soccerClient.Disconnect();
        }
    }
}
