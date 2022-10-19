using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Lemoncode.SqlClient.Soccer
{
    public class SoccerClient
    {
        private readonly string _databaseName;
        private readonly SqlConnection _sqlConnection;

        public SoccerClient(string connectionString, string databaseName)
        {
            _databaseName = databaseName;
            _sqlConnection = new SqlConnection(connectionString);
        }

        public Task Connect()
        {
            return _sqlConnection.OpenAsync();
        }

        public Task DeleteDatabase()
        {
            var sqlDropDatabase = $"DROP DATABASE IF EXISTS {_databaseName};";
            var sqlDropDatabaseCommand = new SqlCommand(sqlDropDatabase, _sqlConnection);
            return sqlDropDatabaseCommand.ExecuteNonQueryAsync();
        }

        public async Task CreateDatabase()
        {
            var sqlCreateDatabase = $"CREATE DATABASE {_databaseName};";
            var sqlCreateDatabaseCommand = new SqlCommand(sqlCreateDatabase, _sqlConnection);
            await sqlCreateDatabaseCommand.ExecuteNonQueryAsync();

            var sqlUseDatabase = $"USE {_databaseName};";
            var sqlUseDatabaseCommand = new SqlCommand(sqlUseDatabase, _sqlConnection);
            sqlUseDatabaseCommand.ExecuteNonQuery();
        }

        public async Task CreateTables()
        {
            var sqlCreateTableTeam =
                "CREATE TABLE Team ("
                + "Id INT NOT NULL,"
                + "Code CHAR(3) NOT NULL,"
                + "PRIMARY KEY(Id));";
            var sqlCreateTableOneCommand = new SqlCommand(sqlCreateTableTeam, _sqlConnection);
            await sqlCreateTableOneCommand.ExecuteNonQueryAsync();

            var sqlCreateTableGame =
                "CREATE TABLE Game ("
                + "Id INT NOT NULL,"
                + "HomeTeamId INT NOT NULL,"
                + "AwayTeamId INT NOT NULL,"
                + "ScheduledOn DATETIME NOT NULL,"
                + "StartedOn DATETIME NULL,"
                + "EndedOn DATETIME NULL,"
                + "PRIMARY KEY(Id),"
                + "FOREIGN KEY (HomeTeamId) REFERENCES Team(Id),"
                + "FOREIGN KEY (AwayTeamId) REFERENCES Team(Id));";
            var sqlCreateTableTwoCommand = new SqlCommand(sqlCreateTableGame, _sqlConnection);
            await sqlCreateTableTwoCommand.ExecuteNonQueryAsync();

            var sqlCreateTableGoal =
                "CREATE TABLE Goal ("
                + "Id INT NOT NULL,"
                + "TeamId INT NOT NULL,"
                + "GameId INT NOT NULL,"
                + "ScoredBy VARCHAR(100) NOT NULL,"
                + "ScoredOn DATETIME NOT NULL,"
                + "PRIMARY KEY(Id),"
                + "FOREIGN KEY (TeamId) REFERENCES Team(Id),"
                + "FOREIGN KEY (GameId) REFERENCES Game(Id));";
            var sqlCreateTableJoinCommand = new SqlCommand(sqlCreateTableGoal, _sqlConnection);
            await sqlCreateTableJoinCommand.ExecuteNonQueryAsync();
        }

        public async Task InsertData()
        {
            var sqlInsertTeams =
                "INSERT INTO Team([Id],[Code]) VALUES"
                + "(1, 'RMA'),"
                + "(2, 'BAR'),"
                + "(3, 'MGA'),"
                + "(4, 'ATM'),"
                + "(5, 'ALV'),"
                + "(6, 'PAL'),"
                + "(7, 'SEV'),"
                + "(8, 'DEP'),"
                + "(9, 'CEL'),"
                + "(10, 'VAD'),"
                + "(11, 'GRA'),"
                + "(12, 'OSA'),"
                + "(13, 'RSO'),"
                + "(14, 'RAC'),"
                + "(15, 'COR'),"
                + "(16, 'EIB'),"
                + "(17, 'RAY'),"
                + "(18, 'VAL'),"
                + "(20, 'VIL'),"
                + "(19, 'ESP');";
            var sqlInsertTeamsCommand = new SqlCommand(sqlInsertTeams, _sqlConnection);
            await sqlInsertTeamsCommand.ExecuteNonQueryAsync();

            var sqlInsertGames =
                "INSERT INTO Game([Id],[HomeTeamId],[AwayTeamId],[ScheduledOn],[StartedOn],[EndedOn]) VALUES"
                + "(1, 1, 2, '2021-10-27 18:00:00.000', '2021-10-27 18:00:10.100', null),"
                + "(2, 3, 4, '2021-10-27 18:00:00.000', '2021-10-27 18:00:20.200', null),"
                + "(3, 5, 6, '2021-10-27 20:00:00.000', null, null),"
                + "(4, 7, 8, '2021-10-28 18:00:00.000', null, null),"
                + "(5, 9, 10, '2021-10-28 20:00:00.000', null, null);";
            var sqlInsertGamesCommand = new SqlCommand(sqlInsertGames, _sqlConnection);
            await sqlInsertGamesCommand.ExecuteNonQueryAsync();

            var sqlInsertGoals =
                "INSERT INTO Goal([Id],[TeamId],[GameId],[ScoredBy],[ScoredOn]) VALUES"
                + "(1, 1, 1, 'Karim Benzema', '2021-10-27 18:05:25.000'),"
                + "(2, 1, 1, 'Vinicius Jr', '2021-10-27 18:07:10.000'),"
                + "(3, 3, 2, 'Antonio Cortés', '2021-10-27 18:07:15.000'),"
                + "(4, 4, 2, 'Luis Suarez', '2021-10-27 18:10:00.200');";
            var sqlInsertGoalsCommand = new SqlCommand(sqlInsertGoals, _sqlConnection);
            await sqlInsertGoalsCommand.ExecuteNonQueryAsync();
        }
        
        public async Task QueryData(Action<string> linePrinter)
        {
            var sqlQueryTeams =
                "SELECT t.Code "
                + "FROM Team t;";
            var sqlQueryTeamsCommand = new SqlCommand(sqlQueryTeams, _sqlConnection);
            var teamsReader = await sqlQueryTeamsCommand.ExecuteReaderAsync();
            linePrinter.Invoke("\nTeams:");
            while (await teamsReader.ReadAsync())
            {
                var code = teamsReader.GetString(0);
                linePrinter.Invoke(code);
            }
            teamsReader.Close();

            var sqlQueryGames =
                "SELECT th.Code, ta.Code, g.StartedOn "
                + "FROM Game AS g "
                + "JOIN Team AS th On g.HomeTeamId = th.Id "
                + "JOIN Team AS ta On g.AwayTeamId = ta.Id "
                + "WHERE g.StartedOn IS NOT null;";
            var sqlQueryGamesCommand = new SqlCommand(sqlQueryGames, _sqlConnection);
            var gamesReader = await sqlQueryGamesCommand.ExecuteReaderAsync();
            linePrinter.Invoke("\nGames:");
            while (await gamesReader.ReadAsync())
            {
                var teamHomeCode = gamesReader.GetString(0);
                var teamAwayCode = gamesReader.GetString(1);
                var startedOn = gamesReader.GetDateTime(2);
                linePrinter.Invoke($"{teamHomeCode} - {teamAwayCode} started on {startedOn}");
            }
            gamesReader.Close();
        }

        public async Task Disconnect()
        {
            await _sqlConnection.CloseAsync();
            await _sqlConnection.DisposeAsync();
        }
    }
}
