using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker.harris_andy
{
    public class DisplayData
    {
        public static void CreateTable(List<CodingSession> sessions)
        {
            var table = new Table();
            bool isAlternateRow = false;

            table.BorderColor(Color.DarkSlateGray1);
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("[cyan1]ID[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue1]Activity[/]").RightAligned());
            table.AddColumn(new TableColumn("[green1]Start Day[/]").RightAligned());
            table.AddColumn(new TableColumn("[green1]Start Time[/]").RightAligned());
            table.AddColumn(new TableColumn("[red1]End Day[/]").RightAligned());
            table.AddColumn(new TableColumn("[red1]End Time[/]").RightAligned());
            table.AddColumn(new TableColumn("[yellow1]Duration (min)[/]").LeftAligned());

            foreach (var session in sessions)
            {
                var color = isAlternateRow ? "grey" : "blue";
                table.AddRow(
                    $"[{color}]{session.Id}[/]",
                    $"[{color}]{session.Activity ?? "N/A"}[/]",
                    $"[{color}]{session.StartDateTime.ToShortDateString()}[/]",
                    $"[{color}]{session.StartDateTime.ToShortTimeString()}[/]",
                    $"[{color}]{session.EndDateTime.ToShortDateString()}[/]",
                    $"[{color}]{session.EndDateTime.ToShortTimeString()}[/]",
                    $"[{color}]{session.Duration} min[/]"
                );
                isAlternateRow = !isAlternateRow;
            }
            Console.Clear();
            AnsiConsole.Write(table);
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        public static void CreateTableFiltered(List<SummaryReport> reports, string dateFilter)
        {
            Console.Clear();
            var table = new Table();
            bool isAlternateRow = false;

            table.BorderColor(Color.DarkSlateGray1);
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn($"[cyan1]{dateFilter}[/]").LeftAligned());
            table.AddColumn(new TableColumn("[green1]Total Time (min)[/]").RightAligned());
            table.AddColumn(new TableColumn("[green1]Avg Time (min)[/]").RightAligned());
            table.AddColumn(new TableColumn("[blue1]Sessions[/]").RightAligned());
            table.AddColumn(new TableColumn("[magenta1]Activities[/]").LeftAligned());

            foreach (var row in reports)
            {
                string dateRange = row.DateRange;
                if (dateFilter == "Week")
                {
                    // Converting row.DateRange into an actual date was weird and I didn't understand it so AI helped. My bad.
                    var parts = dateRange.Split('-');
                    int year = int.Parse(parts[0]);
                    int weekNumber = int.Parse(parts[1]);

                    DateTime jan1 = new DateTime(year, 1, 1);
                    DateTime startOfWeek = jan1.AddDays((weekNumber - 1) * 7 - (int)jan1.DayOfWeek + (int)DayOfWeek.Monday);
                    dateRange = startOfWeek.ToString("yyyy-MM-dd");
                }

                string formattedActivities = string.Join(", ", row.Activity.Split(','));
                var color = isAlternateRow ? "grey" : "blue";

                table.AddRow(
                    $"[{color}]{dateRange}[/]",
                    $"[{color}]{row.TotalTime:F1}[/]",
                    $"[{color}]{row.AvgTime:F1}[/]",
                    $"[{color}]{row.Sessions}[/]",
                    $"[{color}]{formattedActivities}[/]"
                );
                isAlternateRow = !isAlternateRow;
            }
            AnsiConsole.Write(table);
            Console.WriteLine("Press any key to continue...");
            Console.Read();
            Console.Clear();
        }

        public static void CreateTableCodingGoals(List<CodingGoal> codingGoals)
        {
            var table = new Table();
            bool isAlternateRow = false;

            table.BorderColor(Color.DarkSlateGray1);
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("[cyan1]ID[/]").LeftAligned());
            table.AddColumn(new TableColumn("[green1]Start Date[/]").RightAligned());
            table.AddColumn(new TableColumn("[blue1]End Date[/]").RightAligned());
            table.AddColumn(new TableColumn("[yellow1]Goal Hours[/]").RightAligned());
            table.AddColumn(new TableColumn("[red]Complete?[/]").LeftAligned());

            foreach (var goal in codingGoals)
            {
                var color = isAlternateRow ? "grey" : "blue";
                table.AddRow(
                    $"[{color}]{goal.Id}[/]",
                    $"[{color}]{goal.GoalStartDate.ToShortDateString()}[/]",
                    $"[{color}]{goal.GoalEndDate.ToShortDateString()}[/]",
                    $"[{color}]{goal.GoalHours}[/]",
                    $"[{color}]{goal.Complete}[/]"
                );
                isAlternateRow = !isAlternateRow;
            }
            Console.Clear();
            AnsiConsole.Write(table);
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        public static void LiveCodingSession()
        {
            Console.Clear();
            string activity = UserInput.GetActivity();
            DateTime startTime = DateTime.Now;
            string startDayTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
            string endDayTime = "1001-01-01 00:00:01";

            AnsiConsole.Status()
                .Start("Coding...", ctx =>
                {
                    while (true)
                    {
                        var elapsedTime = DateTime.Now - startTime;
                        ctx.Status($"Coding...  {elapsedTime.ToString(@"hh\:mm\:ss")}");
                        ctx.Spinner(Spinner.Known.Pong);
                        ctx.SpinnerStyle(Style.Parse("yellow"));

                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo button = Console.ReadKey(true);
                            if (button.Key == ConsoleKey.Spacebar)
                            {
                                endDayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            }
                        }
                    }
                });
            CodingSession stopwatch = new CodingSession(startDayTime, endDayTime, activity);
            DBInteractions.Insert(stopwatch);
            Console.Clear();
        }

        public static void ShowCodingGoalProgress(CodingGoal goal, SummaryReport sessionData)
        {
            Console.Clear();
            var table = new Table();
            double codingTime = Convert.ToDouble(sessionData.TotalTime / 60);
            double goalTime = Convert.ToDouble(goal.GoalHours);
            Text goalMessage;

            AnsiConsole.Write(new Rule("[yellow bold]Coding Goal Summary[/]")
            {
                Style = Style.Parse("yellow"),
                Justification = Justify.Center,
                Border = BoxBorder.Heavy
            });
            AnsiConsole.WriteLine();

            table.BorderColor(Color.DarkSlateGray1);
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("[cyan1]Start Date[/]").LeftAligned());
            table.AddColumn(new TableColumn("[green1]End Date[/]").RightAligned());
            table.AddColumn(new TableColumn("[red]Complete?[/]").LeftAligned());

            table.AddRow(
                $"{goal.GoalStartDate.ToShortDateString()}",
                $"{goal.GoalEndDate.ToShortDateString()}",
                $"{goal.Complete}"
            );
            AnsiConsole.Write(table);
            Console.WriteLine("\n");

            if (codingTime < goalTime)
            {
                double hoursPerDayNeeded = RetrieveRecord.GetGoalTimeNeeded(goal, sessionData);
                goalMessage = new Text($"You need to code {hoursPerDayNeeded:F1} hours/day to reach your goal.", new Style(Color.Red));
            }
            else
            {
                goalMessage = new Text($"You've reached your goal. Hooray!", new Style(Color.Yellow));
            }
            var pad_I = new Padder(goalMessage).PadLeft(5);
            AnsiConsole.Write(pad_I);

            AnsiConsole.Write(new BarChart()
                .Width(75)
                .Label("[green bold underline]Coding Hours[/]")
                .CenterLabel()
                .AddItem("Coding Time", codingTime, Color.Red)
            .AddItem("Goal:", goalTime, Color.Blue));
            Console.WriteLine("\n\n");
        }
    }
}