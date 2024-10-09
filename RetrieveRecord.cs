using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

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
            string filterOption = UserInput.FilteredOptionsMenu();
            (string[] _, List<CodingSession> sessions) = GetRecords();
            string[] columnHeaders = ["", "Sessions", "Total Time", "Avg. Min/Session", "Activities"];
            columnHeaders[0] = filterOption == "day" ? "Day"
                    : filterOption == "week" ? "Week Starting"
                    : "Year";

            var groupedByDay = sessions
                .GroupBy(session => session.StartDateTime.Date)
                .OrderBy(s => s.Key)
                .ToList();

            var groupedByWeek = sessions
                .GroupBy(session =>
                {
                    var startDate = session.StartDateTime.Date;
                    var weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(startDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    var year = startDate.Year;
                    return new DateTime(year, 1, 1).AddDays((weekOfYear - 1) * 7);
                })
                .OrderBy(s => s.Key)
                .ToList();

            var groupedByYear = sessions
                .GroupBy(session => new DateTime(session.StartDateTime.Year, 1, 1))
                .OrderBy(g => g.Key)
                .ToList();

            if (filterOption == "day")
                UserInput.CreateTableFiltered(columnHeaders, groupedByDay, "day");
            if (filterOption == "week")
                UserInput.CreateTableFiltered(columnHeaders, groupedByWeek, "week");
            if (filterOption == "year")
                UserInput.CreateTableFiltered(columnHeaders, groupedByYear, "year");
        }

        public static void GetRecordSummary()
        {

        }
    }
}