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
        public static List<CodingSession> GetRecords()
        {
            string answer = UserInput.GetAllOrFiltered();

            if (answer == "Filtered")
            {
                GetFilteredRecords();
            }

            var sql = "SELECT Id, StartDayTime, EndDayTime, Activity FROM coding";
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var sessions = connection.Query<CodingSession>(sql).ToList();

            UserInput.CreateTable(sessions);
            Console.WriteLine("Press any key to continue...");
            Console.Read();

            return sessions;
        }
        // Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.

        public static void GetFilteredRecords()
        {
            string filterOption = UserInput.FilteredOptionsMenu();

        }

        public static void GetRecordSummary()
        {

        }
    }
}