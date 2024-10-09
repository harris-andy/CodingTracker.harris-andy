using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.harris_andy
{
    public class RetrieveRecord
    {
        public static List<CodingSession> GetRecords()
        {
            var sql = "SELECT Id, StartDayTime, EndDayTime, Activity FROM coding";
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            List<CodingSession> sessions = connection.Query<CodingSession>(sql).ToList();
            // string[] columns = ["ID", "Activity", "Start Day", "Start Time", "End Day", "End Time", "Duration"];

            // UserInput.CreateTable(columns, sessions);
            // Console.WriteLine("Press any key to continue...");
            // Console.Read();

            return sessions;
        }

        public static void GetFilteredRecords()
        {
            /*
                SQL STRING FOR "WEEK STARTING DATE"

                strftime('%Y-%m-%d', 
                    date(strftime('%Y-01-01', StartDayTime), 
                        'weekday 0', 
                        '+' || (strftime('%W', StartDayTime)) || ' weeks')
                ) as WeekStart
            */
            string filter = UserInput.FilteredOptionsMenu();
            Dictionary<string, string> dateRange = new Dictionary<string, string>
            {
                {"day", "'%Y-%m-%d'"},
                {"week", "'%Y-%W'"},
                {"year", "'%Y'"}
            };

            var reportQuery = @$"
            WITH DayData AS (
                SELECT 
                    strftime({dateRange[$"{filter.ToLower()}"]}, StartDayTime) AS DateRange,
                    ROUND(SUM((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS TotalTime,
                    ROUND(AVG((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS AvgTime,
                    COUNT(*) AS Sessions,
                    GROUP_CONCAT(DISTINCT Activity) as Activity
                    FROM coding
                    GROUP BY strftime({dateRange[$"{filter.ToLower()}"]}, StartDayTime)
                    ORDER BY DateRange
                    )
                SELECT 
                    DateRange,
                    TotalTime,
                    AvgTime,
                    Sessions,
                    Activity
                FROM DayData";

            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            List<SummaryReport> reports = connection.Query<SummaryReport>(reportQuery).ToList();
            DisplayData.CreateTableFiltered(reports, filter);
        }

        public static void GetRecordSummary()
        {

        }
    }
}