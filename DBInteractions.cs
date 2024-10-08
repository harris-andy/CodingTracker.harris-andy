using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using Dapper;


namespace CodingTracker.harris_andy
{
    public class DBInteractions
    {
        public static void Insert(DateTime startDateTime, DateTime endDateTime, string activity)
        {
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var parameters = new { start = startDateTime, end = endDateTime, activityEntry = activity };
            var sql = "INSERT INTO coding (StartDayTime, EndDayTime, Activity) VALUES (@start, @end, @activityEntry)";
            var result = connection.Query(sql, parameters);

            // Console.WriteLine("");
            // Console.WriteLine($"Record insert! More data is gooder data.");
            // Console.WriteLine("");
            // Thread.Sleep(2000);
        }

        public static void Delete()
        {
            (int recordID, bool confirmation) = UserInput.GetRecordID("delete");

            if (confirmation)
            {
                using var connection = new SqliteConnection(AppConfig.ConnectionString);
                var parameters = new { Id = recordID };
                var sql = "DELETE FROM coding WHERE Id = @Id";
                var result = connection.Query(sql, parameters);

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

        public static void Update()
        {
            (int recordID, bool confirmation) = UserInput.GetRecordID("update");

            if (confirmation)
            {
                (DateTime startDateTime, DateTime endDateTime, string activity) = UserInput.GetSessionData();
                using var connection = new SqliteConnection(AppConfig.ConnectionString);
                var parameters = new { id = recordID, start = startDateTime, end = endDateTime, activityEntry = activity };
                var sql = "UPDATE coding SET StartDayTime = @start, EndDayTime = @end, Activity = @activityEntry WHERE Id = @id";
                var result = connection.Query(sql, parameters);

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

        public static void DeleteTableContents()
        {
            Console.Clear();
            string? verify = null;
            string[] prompts = {
                "Are you SURE you want to delete all this data? y/n",
                "Are you REALLY SUPER SURE? y/n",
                "Last chance to turn back! Are you ABSOLUTELY TOTALLY POSITIVELY SURE? y/n"
            };

            foreach (string prompt in prompts)
            {
                Console.WriteLine(prompt);
                verify = Console.ReadLine()?.ToLower();
                if (verify != "y")
                {
                    return;
                }
            }

            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            connection.Open();
            using (var command = new SqliteCommand("DELETE FROM coding;", connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();

            Console.WriteLine("\n\n");
            Console.WriteLine("Whelp. It's all gone. Now what...?");
            Thread.Sleep(2000);
        }

        public static void PopulateDatabase()
        {
            Random random = new Random();
            string[] programmingActivities = ["c#", "python", "c++", "javascript", "frontend", "backend"];

            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            connection.Open();

            for (int randomRecord = 0; randomRecord < 100; randomRecord++)
            {
                DateTime randomStartDay = RandomDateTime.RandomDate();
                DateTime randomStartDateTime = RandomDateTime.RandomStartDateTime(randomStartDay);
                DateTime randomEndDateTime = RandomDateTime.RandomEndDateTime(randomStartDateTime);
                string activity = programmingActivities[random.Next(programmingActivities.Length)];

                Insert(randomStartDateTime, randomEndDateTime, activity);
            }
            connection.Close();
        }

        public static void InitializeDatabase()
        {
            using SqliteConnection connection = new SqliteConnection(AppConfig.ConnectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartDayTime TEXT,
                EndDayTime TEXT,
                Activity TEXT
                )";

            // StartTime TEXT,
            // EndTime TEXT,

            tableCmd.ExecuteNonQuery();

            // DBInteractions.DeleteTableContents();

            using var command = new SqliteCommand("SELECT COUNT(*) FROM coding;", connection);

            var count = Convert.ToInt32(command.ExecuteScalar());
            if (count == 0)
            {
                PopulateDatabase();
            }
            connection.Close();
        }
    }
}