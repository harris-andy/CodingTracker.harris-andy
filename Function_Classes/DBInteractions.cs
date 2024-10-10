using Microsoft.Data.Sqlite;
using Spectre.Console;
using Dapper;


namespace CodingTracker.harris_andy
{
    public class DBInteractions
    {
        public static void InitializeCodingGoalDatabase()
        {
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var createTable = @"CREATE TABLE IF NOT EXISTS coding_goals (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                GoalStartDate TEXT,
                GoalEndDate TEXT,
                GoalHours TEXT,
                Complete TEXT
                )";
            connection.Execute(createTable);
        }

        public static void InitializeDatabase()
        {
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var createTable = @"CREATE TABLE IF NOT EXISTS coding (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartDayTime TEXT,
                EndDayTime TEXT,
                Activity TEXT
                )";
            connection.Execute(createTable);

            string sql = "SELECT COUNT(*) FROM coding";
            int count = connection.ExecuteScalar<int>(sql);
            if (count == 0)
                PopulateDatabase();
        }

        public static void Insert(CodingSession session)
        {
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var parameters = new { start = session.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"), end = session.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss"), activityEntry = session.Activity };
            var sql = "INSERT INTO coding (StartDayTime, EndDayTime, Activity) VALUES (@start, @end, @activityEntry)";
            connection.Execute(sql, parameters);
        }

        public static void InsertCodingGoal(CodingGoal goal)
        {
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var parameters = new { start = goal.GoalStartDate.ToString("yyyy-MM-dd HH:mm:ss"), end = goal.GoalEndDate.ToString("yyyy-MM-dd HH:mm:ss"), hours = goal.GoalHours, complete = "no" };
            var sql = "INSERT INTO coding_goals (GoalStartDate, GoalEndDate, GoalHours, Complete) VALUES (@start, @end, @hours, 'no')";
            connection.Execute(sql, parameters);
        }

        public static void Delete()
        {
            (int recordID, bool confirmation) = UserInput.GetRecordID("delete", "session");

            if (confirmation)
            {
                using var connection = new SqliteConnection(AppConfig.ConnectionString);
                var parameters = new { Id = recordID };
                var sql = "DELETE FROM coding WHERE Id = @Id";
                connection.Execute(sql, parameters);

                Console.WriteLine("");
                Console.WriteLine($"Record {recordID} deleted! It was a stupid record anyway.");
                Console.WriteLine("");
                Thread.Sleep(2000);
            }
            else
            {
                UserInput.MainMenu();
            }
            Console.Clear();
        }

        public static void DeleteTableContents()
        {
            Console.Clear();
            bool confirmation = false;
            string[] prompts = {
                "Are you SURE you want to delete all this data? y/n",
                "Are you REALLY SUPER SURE? y/n",
                "Last chance to turn back! Are you ABSOLUTELY TOTALLY POSITIVELY SURE? y/n"
            };

            foreach (string prompt in prompts)
            {
                confirmation = AnsiConsole.Prompt(
                new TextPrompt<bool>(prompt)
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));
            }

            if (confirmation)
            {
                using var connection = new SqliteConnection(AppConfig.ConnectionString);
                var sql = @"
                    DELETE FROM coding;
                    DELETE FROM sqlite_sequence WHERE name = 'coding'";
                connection.Execute(sql);

                Console.WriteLine("\n\n");
                Console.WriteLine("Whelp. It's all gone. I hope none of that was important");
                Thread.Sleep(2000);
            }
            Console.Clear();
        }

        public static void DropCodingGoalsTable()
        {
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var dropTable = @"DROP TABLE coding_goals";
            connection.Execute(dropTable);
        }

        public static void PopulateDatabase()
        {
            Random random = new Random();
            string[] programmingActivities = ["c#", "python", "c++", "javascript", "frontend", "backend"];

            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            for (int i = 0; i < 100; i++)
            {
                DateTime randomStartDay = RandomDateTime.RandomDate();
                DateTime randomStartDateTime = RandomDateTime.RandomStartDateTime(randomStartDay);
                DateTime randomEndDateTime = RandomDateTime.RandomEndDateTime(randomStartDateTime);
                string activity = programmingActivities[random.Next(programmingActivities.Length)];
                CodingSession session = new CodingSession
                {
                    StartDayTime = randomStartDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDayTime = randomEndDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Activity = activity
                };
                Insert(session);
            }
            Console.Clear();
        }

        public static void Update()
        {
            (int recordID, bool confirmation) = UserInput.GetRecordID("update", "session");

            if (confirmation)
            {
                CodingSession session = UserInput.GetSessionData();
                using var connection = new SqliteConnection(AppConfig.ConnectionString);
                var parameters = new { id = recordID, start = session.StartDateTime, end = session.EndDateTime, activityEntry = session.Activity };
                var sql = "UPDATE coding SET StartDayTime = @start, EndDayTime = @end, Activity = @activityEntry WHERE Id = @id";
                connection.Execute(sql, parameters);

                Console.WriteLine("");
                Console.WriteLine($"Record {recordID} updated! We're having so much fun.");
                Console.WriteLine("");
                Thread.Sleep(2000);
            }
            else
            {
                UserInput.MainMenu();
            }
            Console.Clear();
        }

        public static void UpdateCodingGoalComplete(int goalId)
        {
            string sql = $"UPDATE coding_goals SET Complete = 'yes' WHERE Id = {goalId}";
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            connection.Execute(sql);
        }
    }
}