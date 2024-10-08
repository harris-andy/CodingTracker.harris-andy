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
        // Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.

        public static void GetFilteredRecords()
        {
            string filterOption = UserInput.FilteredOptionsMenu();
            (string[] columnsDefault, List<CodingSession> sessions) = GetRecords();
            string[] columnsDays = ["Day", "Sessions", "Total Time", "Avg. Min/Session"];

            var sessionsByDay = sessions
                .GroupBy(session => session.StartDateTime.Date)
                .OrderBy(s => s.Key)
                .ToList();

            var groupedByWeek = sessions
                .GroupBy(session =>
                    CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                        session.StartDateTime.Date,
                        CalendarWeekRule.FirstFourDayWeek,
                        DayOfWeek.Monday))
                .OrderBy(s => s.Key)
                .ToList();

            var groupedByYear = sessions
                .GroupBy(session => session.StartDateTime.Year)
                .OrderBy(s => s.Key)
                .ToList();

            if (filterOption == "day")
                UserInput.CreateTableFiltered(columnsDays, sessionsByDay);
        }

        public static void GetRecordSummary()
        {

        }
    }
}