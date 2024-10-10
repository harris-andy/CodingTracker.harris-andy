using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static void ShowFilteredRecords()
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
            Dictionary<string, string> dateType = new Dictionary<string, string>
            {
                {"day", "'%Y-%m-%d'"},
                {"week", "'%Y-%W'"},
                {"year", "'%Y'"}
            };

            var reportQuery = @$"
            WITH DateRangeData AS (
                SELECT 
                    strftime({dateType[$"{filter.ToLower()}"]}, StartDayTime) AS DateRange,
                    ROUND(SUM((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS TotalTime,
                    ROUND(AVG((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS AvgTime,
                    COUNT(*) AS Sessions,
                    GROUP_CONCAT(DISTINCT Activity) as Activity
                FROM coding
                GROUP BY strftime({dateType[$"{filter.ToLower()}"]}, StartDayTime)
                ORDER BY DateRange
                    )
                SELECT 
                    DateRange,
                    TotalTime,
                    AvgTime,
                    Sessions,
                    Activity
                FROM DateRangeData";

            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            List<SummaryReport> reports = connection.Query<SummaryReport>(reportQuery).ToList();
            DisplayData.CreateTableFiltered(reports, filter);
        }

        public static List<CodingGoal> GetCodingGoals()
        {
            string codingSQL = "SELECT * FROM coding_goals";
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            List<CodingGoal> goals = connection.Query<CodingGoal>(codingSQL).ToList();

            foreach (CodingGoal goal in goals)
            {
                SummaryReport sessionData = GetCodingGoalSessionData(goal);
                if ((float)sessionData.TotalTime / 60 > goal.GoalHours)
                {
                    DBInteractions.UpdateCodingGoalComplete(goal.Id);
                }
            }
            return goals;
        }

        public static void GetCodingGoalProgressData()
        {
            List<CodingGoal> _ = GetCodingGoals();

            (int recordID, bool _) = UserInput.GetRecordID("get progress for", "goal");
            string codingSQL = $"SELECT * FROM coding_goals WHERE Id = {recordID}";
            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            CodingGoal goal = connection.QuerySingle<CodingGoal>(codingSQL);

            SummaryReport sessionData = GetCodingGoalSessionData(goal);

            DisplayData.ShowCodingGoalProgress(goal, sessionData);

            Console.WriteLine("Press any key to continue...");
            Console.Read();
            Console.Clear();
        }

        public static SummaryReport GetCodingGoalSessionData(CodingGoal goal)
        {
            string startDate = goal.GoalStartDate.ToString("yyyy-MM-dd HH:mm:ss");
            string endDate = goal.GoalEndDate.ToString("yyyy-MM-dd HH:mm:ss");

            string progressSQL = @$"
                SELECT
                    '{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}' AS DateRange,
                    ROUND(SUM((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS TotalTime,
                    ROUND(AVG((julianday(EndDayTime) - julianday(StartDayTime)) * 24 * 60), 2) AS AvgTime,
                    COUNT (*) AS Sessions,
                    GROUP_CONCAT(DISTINCT Activity) as Activity
                FROM coding
                WHERE
                    StartDayTime BETWEEN '{startDate}' AND '{endDate}'
                    AND EndDayTime BETWEEN '{startDate}' AND '{endDate}'";

            using var connection = new SqliteConnection(AppConfig.ConnectionString);
            SummaryReport sessionData = connection.QuerySingle<SummaryReport>(progressSQL);

            return sessionData;
        }
    }
}