// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace CodingTracker.harris_andy
// {
//     public class junk
//     {
//         public static void GetFilteredRecords()
//         {
//             string filterOption = UserInput.FilteredOptionsMenu();
//             (string[] _, List<CodingSession> sessions) = GetRecords();
//             string[] columnHeaders = ["", "Sessions", "Total Time", "Avg. Min/Session", "Activities"];
//             columnHeaders[0] = filterOption == "day" ? "Day"
//                     : filterOption == "week" ? "Week Starting"
//                     : "Year";

//             var groupedByDay = sessions
//                 .GroupBy(session => session.StartDateTime.Date)
//                 .OrderBy(s => s.Key)
//                 .ToList();

//             var groupedByWeek = sessions
//                 .GroupBy(session =>
//                 {
//                     var startDate = session.StartDateTime.Date;
//                     var weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(startDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
//                     var year = startDate.Year;
//                     return new DateTime(year, 1, 1).AddDays((weekOfYear - 1) * 7);
//                 })
//                 .OrderBy(s => s.Key)
//                 .ToList();

//             var groupedByYear = sessions
//                 .GroupBy(session => new DateTime(session.StartDateTime.Year, 1, 1))
//                 .OrderBy(g => g.Key)
//                 .ToList();

//             if (filterOption == "day")
//                 UserInput.CreateTableFiltered(columnHeaders, groupedByDay, "day");
//             if (filterOption == "week")
//                 UserInput.CreateTableFiltered(columnHeaders, groupedByWeek, "week");
//             if (filterOption == "year")
//                 UserInput.CreateTableFiltered(columnHeaders, groupedByYear, "year");
//         }

//         public static void CreateTableFiltered(string[] columns, List<IGrouping<DateTime, CodingSession>> sessions, string dateFormat)
//         {
//             var table = new Table();

//             foreach (string header in columns)
//             {
//                 table.AddColumn(header).Centered();
//             }

//             foreach (var group in sessions)
//             {
//                 int totalMinutes = group.Sum(session => session.Duration);
//                 float averageMinutes = totalMinutes / (float)group.Count();
//                 TimeSpan totalTime = TimeSpan.FromMinutes(totalMinutes);
//                 int hours = (int)totalTime.TotalHours;
//                 int minutes = totalTime.Minutes;
//                 string dateRow = dateFormat == "year" ? group.Key.ToString("yyyy") : group.Key.ToShortDateString();
//                 var activities = group
//                         .Where(session => !string.IsNullOrEmpty(session.Activity))
//                         .Select(session => session.Activity!)
//                         .ToList();

//                 HashSet<string> uniqueActivites = new HashSet<string>(activities);
//                 string activitiesList = string.Join(", ", uniqueActivites);

//                 table.AddRow(
//                     dateRow,
//                     group.Count().ToString(),
//                     $"{hours}h {minutes}m",
//                     $"{averageMinutes:F1}",
//                     activitiesList
//                 );
//             }
//             Console.Clear();
//             AnsiConsole.Write(table);
//             Console.WriteLine("Press any key to continue...");
//             Console.Read();
//         }


//     }
// }