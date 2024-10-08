using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace CodingTracker.harris_andy
{
    public class DBInteractions
    {
        public static void Insert(DateTime startDateTime, DateTime endDateTime, string activity)
        {
            // Console.Clear();
            // DateTime date = Spectre.GetDate();
            // Console.Clear();
            // TimeSpan startTime = Spectre.GetTime("start");
            // Console.Clear();
            // TimeSpan endTime = Spectre.GetTime("end");
            // Console.Clear();
            // string activity = Spectre.GetActivity();
            // (DateTime startDateTime, DateTime endDateTime) = Spectre.ParseDateTimes(date, startTime, endTime);

            using var connection = new SqliteConnection(AppConfig.ConnectionString);

            connection.Open();
            using (var command = new SqliteCommand("INSERT INTO coding (StartDayTime, EndDayTime, Activity) VALUES (@start, @end, @activity)", connection))
            {
                command.Parameters.AddWithValue("@start", startDateTime);
                command.Parameters.AddWithValue("@end", endDateTime);
                command.Parameters.AddWithValue("@activity", activity);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public static void Delete()
        {
            // Console.Clear();
            // bool withIds = true;
            // GetAllRecords(withIds);
            // string message = "Enter the ID number for the record you'd like to delete. Or enter 0 to return to Main Menu.";
            // int recordID = validateNumberEntry(message);

            // using (var connection = new SqliteConnection(connectionString))
            // {
            //     connection.Open();
            //     using (var command = new SqliteCommand("DELETE FROM habits WHERE ID = @id", connection))
            //     {
            //         command.Parameters.AddWithValue("@id", recordID);
            //         command.ExecuteNonQuery();
            //     }
            //     connection.Close();
            // }
            // Console.WriteLine($"Record ID {recordID} has been deleted.");
        }

        public static void Update()
        {
            // Console.Clear();
            // bool withIds = true;
            // bool recordExists = false;
            // GetAllRecords(withIds);
            // string message = "Enter the ID number for the record you'd like to update. Or enter 0 to return to Main Menu.";
            // int recordID = validateNumberEntry(message);
            // if (recordID == 0) MainMenu();

            // using (var connection = new SqliteConnection(connectionString))
            // {
            //     connection.Open();
            //     using (var command = new SqliteCommand("SELECT * FROM habits WHERE Id = @id", connection))
            //     {
            //         command.Parameters.AddWithValue("@id", recordID);
            //         recordExists = Convert.ToInt32(command.ExecuteScalar()) > 0;
            //     }
            //     connection.Close();
            // }
            // if (!recordExists)
            // {
            //     Console.Clear();
            //     Console.WriteLine("No record found with that ID. Try again.");
            //     Thread.Sleep(2000);
            //     Update();
            // }

            // Console.Clear();
            // Console.WriteLine("Enter the updated record:");
            // string date = GetDateInput();
            // string hobby = GetHobby();
            // string units = GetUnitsInput();
            // int quantity = GetQuantityInput();

            // using (var connection = new SqliteConnection(connectionString))
            // {
            //     connection.Open();
            //     using (var command = new SqliteCommand("UPDATE habits SET date = @date, hobby = @hobby, units = @units, quantity = @quantity WHERE Id = @id", connection))
            //     {
            //         command.Parameters.AddWithValue("@id", recordID);
            //         command.Parameters.AddWithValue("@date", date);
            //         command.Parameters.AddWithValue("@hobby", hobby);
            //         command.Parameters.AddWithValue("@units", units);
            //         command.Parameters.AddWithValue("@quantity", quantity);
            //         command.ExecuteNonQuery();
            //     }
            //     connection.Close();
            // }
            // Console.WriteLine($"Record ID {recordID} has been updated.");
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