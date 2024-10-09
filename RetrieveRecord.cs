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
        public static (string[], List<CodingSession>) GetRecords()
        {
            var sql = "SELECT Id, StartDayTime, EndDayTime, Activity FROM coding";
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            List<CodingSession> sessions = connection.Query<CodingSession>(sql).ToList();
            string[] columns = ["ID", "Activity", "Start Day", "Start Time", "End Day", "End Time", "Duration"];

            // UserInput.CreateTable(columns, sessions);
            // Console.WriteLine("Press any key to continue...");
            // Console.Read();

            return (columns, sessions);
        }

        public static void GetFilteredRecords()
        {
            /*
                Day
                TotalTime
                AvgTime
                Sessions
                Activity
            */
            string filterOption = UserInput.FilteredOptionsMenu();
            var dayGroup = @"
            WITH DayData AS (
                SELECT 
                    strftime('%Y-%m-%d', StartDayTime) AS Day,
                    ROUND(SUM((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS TotalTime,
                    ROUND(AVG((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS AvgTime,
                    COUNT(*) AS Sessions,
                    GROUP_CONCAT(DISTINCT Activity) as Activity
                    FROM coding
                    GROUP BY strftime('%Y-%m-%d', StartDayTime)
                    ORDER BY Day
                    )
                SELECT 
                    Day,
                    TotalTime,
                    AvgTime,
                    Sessions,
                    Activity
                FROM DayData";


            // var yearGroup = @"
            //     SELECT strftime('%Y', StartDayTime) AS Year,
            //     SUM((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60) AS TotalTime,
            //     AVG((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60) AS AvgTime,
            //     COUNT(*) AS Sessions
            //     FROM coding
            //     GROUP BY Year
            //     ORDER BY Year";

            // var weekGroup = @"
            //     SELECT strftime('%Y-%W', StartDayTime) AS Week,
            //     SUM((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60) AS TotalTime,
            //     AVG((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60) AS AvgTime,
            //     COUNT(*) AS Sessions
            //     FROM coding
            //     GROUP BY Week
            //     ORDER BY Week";

            string[] columnHeaders = [$"{filterOption}", "Total Time", "Sessions", "Avg Time", "Activities"];
            // columnHeaders[0] = filterOption == "Day" ? "Day"
            //         : filterOption == "Week" ? "Week Starting"
            //         : "Year";

            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            List<SummaryReport> reports = connection.Query<SummaryReport>(dayGroup).ToList();

            // if (filterOption == "Day")
            UserInput.CreateTableFiltered(reports, filterOption);
            // if (filterOption == "Week")
            //     UserInput.CreateTableFiltered(columnHeaders, groupedByWeek, "Week");
            // if (filterOption == "Year")
            //     UserInput.CreateTableFiltered(columnHeaders, groupedByYear, "Year");
        }

        public static void GetRecordSummary()
        {

        }
    }
}