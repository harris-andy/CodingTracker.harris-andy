using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.harris_andy
{
    public class RetrieveRecord
    {
        public static void GetAllRecords()
        {
            var sql = "SELECT Id, StartDayTime, EndDayTime, Activity FROM coding";
            var sessions = new List<CodingSession>();

            using var connection = new SqliteConnection(AppConfig.ConnectionString);

            sessions = connection.Query<CodingSession>(sql).ToList();

            foreach (var session in sessions)
            {
                Console.WriteLine(session.Duration);
            }
        }

        public static void GetRecordSummary()
        {
            // (string searchTerm, string searchTermCategory) = GetSearchTerm();
            // Console.Clear();

            // using (var connection = new SqliteConnection(connectionString))
            // {
            //     SqliteCommand chosenCommand;
            //     var commandYear = new SqliteCommand("SELECT SUBSTR(Date, 7, 4) AS Year, Hobby, Units, SUM(Quantity) AS TotalQuantity FROM habits WHERE SUBSTR(Date, 7, 4) = @year GROUP BY Year, Hobby;", connection);
            //     var commandHobby = new SqliteCommand("SELECT Hobby, Units, COUNT(*) AS TotalCount, SUM(Quantity) AS TotalUnits FROM habits WHERE Hobby = @hobby GROUP BY Hobby, Units;", connection);
            //     var commandUnits = new SqliteCommand("SELECT SUBSTR(Date, 7, 4) AS Year, Hobby, Units, SUM(Quantity) AS TotalQuantity FROM habits WHERE Units = @units GROUP BY Hobby, Units;", connection);

            //     if (searchTermCategory == "year")
            //     {
            //         chosenCommand = commandYear;
            //         string year = searchTerm;
            //         chosenCommand.Parameters.AddWithValue("@year", year);
            //     }
            //     else if (searchTermCategory == "hobby")
            //     {
            //         chosenCommand = commandHobby;
            //         string hobby = searchTerm;
            //         chosenCommand.Parameters.AddWithValue("@hobby", hobby);
            //     }
            //     else
            //     {
            //         chosenCommand = commandUnits;
            //         string units = searchTerm;
            //         chosenCommand.Parameters.AddWithValue("@units", units);
            //     }

            //     connection.Open();
            //     using (chosenCommand)
            //     {
            //         using (var reader = chosenCommand.ExecuteReader())
            //         {
            //             if (searchTermCategory == "hobby")
            //             {
            //                 if (reader.Read())
            //                 {
            //                     string activity = reader.GetString(0);
            //                     string units = reader.GetString(1);
            //                     int count = reader.GetInt32(2);
            //                     int quantity = reader.GetInt32(3);
            //                     string howManyTimes = count == 1 ? "time" : "times";

            //                     Console.WriteLine("--------------------------------------------------\n");
            //                     Console.WriteLine($"{activity.Substring(0, 1).ToUpper() + activity.Substring(1)} done {count} {howManyTimes} for {quantity} {units}. Nice!\n");
            //                     Console.WriteLine("--------------------------------------------------\n");
            //                 }
            //             }
            //             else
            //             {
            //                 int totalQuantity = 0;
            //                 List<string> actionRecord = new();
            //                 string howManyActivities = actionRecord.Count == 1 ? "activity" : "activities";

            //                 while (reader.Read())
            //                 {
            //                     int year = reader.GetInt32(0);
            //                     string activity = reader.GetString(1);
            //                     string units = reader.GetString(2);
            //                     int quantity = reader.GetInt32(3);
            //                     totalQuantity += quantity;
            //                     actionRecord.Add($"{activity.Substring(0, 1).ToUpper() + activity.Substring(1),-12}: {quantity} {units}");
            //                 }

            //                 string yearOutput = $"Completed {actionRecord.Count} {howManyActivities} in {searchTerm}:\n";
            //                 string unitsOutput = $"{totalQuantity} {searchTerm} completed across {actionRecord.Count} {howManyActivities}:\n";

            //                 Console.WriteLine("\n");
            //                 Console.WriteLine("--------------------------------------------------\n");
            //                 if (searchTermCategory == "year") Console.WriteLine(yearOutput);
            //                 if (searchTermCategory == "units") Console.WriteLine(unitsOutput);
            //                 foreach (string item in actionRecord)
            //                 {
            //                     Console.WriteLine(item);
            //                 }
            //                 Console.WriteLine("\n");
            //                 Console.WriteLine("--------------------------------------------------\n");
            //             }
            //         }
            //     }
            //     connection.Close();
            // }
        }
    }
}