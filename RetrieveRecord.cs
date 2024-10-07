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

            // StartDateTime, EndDateTime, Activity, Duration, Id
            Spectre.CreateTable(sessions);
            Console.WriteLine("Press any key to continue...");
            Console.Read();
            Console.Clear();
        }

        public static void GetRecordSummary()
        {

        }
    }
}