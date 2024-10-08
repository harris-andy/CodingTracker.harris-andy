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
        public static List<CodingSession> GetAllRecords()
        {
            var sql = "SELECT Id, StartDayTime, EndDayTime, Activity FROM coding";
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            var sessions = connection.Query<CodingSession>(sql).ToList();

            // StartDateTime, EndDateTime, Activity, Duration, Id
            UserInput.CreateTable(sessions);
            Console.WriteLine("Press any key to continue...");
            Console.Read();

            return sessions;
        }

        public static void GetRecordSummary()
        {

        }
    }
}