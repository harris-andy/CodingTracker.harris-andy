using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class RetrieveRecord
    {
        public static void GetAllRecords(bool withIds)
        {
            // Console.Clear();
            // string sortMessage = "Sort records by\n1. ID\n2. Date";
            // int sortOption = -1;
            // while (sortOption < 0 || sortOption > 2)
            // {
            //     sortOption = validateNumberEntry(sortMessage);
            // }

            // using (SqliteConnection connection = new SqliteConnection(connectionString))
            // {
            //     List<HobbyRecord> hobbiesSortedID = new List<HobbyRecord>();

            //     connection.Open();
            //     using (SqliteCommand command = new SqliteCommand("SELECT * FROM habits", connection))
            //     {
            //         using (SqliteDataReader reader = command.ExecuteReader())
            //         {
            //             while (reader.Read())
            //             {
            //                 hobbiesSortedID.Add(new HobbyRecord
            //                 {
            //                     Id = reader.GetInt32(0),
            //                     Date = DateTime.ParseExact(reader.GetString(1), format: "dd-MM-yyyy", new CultureInfo("en-US")),
            //                     Hobby = reader.GetString(2),
            //                     Units = reader.GetString(3),
            //                     Quantity = reader.GetInt32(4)
            //                 });
            //             }
            //         }
            //     }
            //     connection.Close();
            //     Console.WriteLine("--------------------------------------------------\n");
            //     Console.WriteLine("Here's all the fun stuff you did!\n");
            //     if (hobbiesSortedID.Count == 0)
            //     {
            //         Console.WriteLine("No records found. Do stuff!");
            //     }
            //     var hobbiesSortedDate = hobbiesSortedID.OrderBy(record => record.Date).ToList();
            //     foreach (HobbyRecord record in sortOption == 1 ? hobbiesSortedID : hobbiesSortedDate)
            //     {
            //         string outputByDate = $"{record.Date.ToString("dd-MMM-yyyy"),-13} {record.Hobby,-14} {record.Units,-5}: {record.Quantity,-5}";
            //         string outputByID = $"{record.Id,-3}: {record.Date.ToString("dd-MMM-yyyy"),-13} {record.Hobby,-14} {record.Units,-5}: {record.Quantity,-5}";
            //         if (sortOption == 1) Console.WriteLine(outputByID);
            //         if (sortOption == 2) Console.WriteLine(outputByDate);
            //     }
            //     Console.WriteLine("--------------------------------------------------\n");
            // }
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