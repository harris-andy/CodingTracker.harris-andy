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
            (string[] columns, List<CodingSession> sessions) = GetRecords();

            var sessionsByDay = sessions
                .GroupBy(session => session.StartDateTime.Date)
                .OrderByDescending(s => s.Key);

            var groupedByWeek = sessions.GroupBy(session =>
                CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    session.StartDateTime.Date,
                    CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday
                ));

            var groupedByYear = sessions.GroupBy(session => session.StartDateTime.Year);

            List<CodingSession> orderedDaySessions = new List<CodingSession>();
            foreach (var group in sessionsByDay)
            {
                foreach (CodingSession session in group)
                {
                    orderedDaySessions.Add(session);
                }
            }

            // foreach (var group in sessionsByDay)
            // {
            //     Console.WriteLine($"Day: {group.Key.ToString("dd-MM-yyyy")}");
            //     int totalMinutes = group.Sum(session => session.Duration);
            //     float averageMinutes = totalMinutes / (float)group.Count();
            //     TimeSpan totalTime = TimeSpan.FromMinutes(totalMinutes);
            //     int hours = (int)totalTime.TotalHours;
            //     int minutes = totalTime.Minutes;
            //     string hoursString = hours == 0 ? "" : $"{hours} hours";
            //     Console.WriteLine($"Total time spent coding: {hoursString} {minutes} minutes");
            //     Console.WriteLine($"Average time per session: {averageMinutes}\n");
            // }
        }

        public static void GetRecordSummary()
        {

        }
    }
}