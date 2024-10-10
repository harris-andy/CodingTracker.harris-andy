using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace CodingTracker.harris_andy
{
    public class DisplayData
    {
        public static void CreateTable(List<CodingSession> sessions)
        {
            var table = new Table();

            table.BorderColor(Color.DarkSlateGray1);
            table.Border(TableBorder.Rounded);

            table.AddColumn(new TableColumn("[cyan1]ID[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue1]Activity[/]").RightAligned());
            table.AddColumn(new TableColumn("[green1]Start Day[/]").RightAligned());
            table.AddColumn(new TableColumn("[green1]Start Time[/]").RightAligned());
            table.AddColumn(new TableColumn("[red1]End Day[/]").RightAligned());
            table.AddColumn(new TableColumn("[red1]End Time[/]").RightAligned());
            table.AddColumn(new TableColumn("[yellow1]Duration (min)[/]").LeftAligned());

            bool isAlternateRow = false;
            foreach (var session in sessions)
            {
                var color = isAlternateRow ? "grey" : "blue";
                table.AddRow(
                    $"[{color}]{session.Id.ToString()}[/]",
                    $"[{color}]{session.Activity ?? "N/A"}[/]",
                    $"[{color}]{session.StartDateTime.ToShortDateString()}[/]",
                    $"[{color}]{session.StartDateTime.ToShortTimeString()}[/]",
                    $"[{color}]{session.EndDateTime.ToShortDateString()}[/]",
                    $"[{color}]{session.EndDateTime.ToShortTimeString()}[/]",
                    $"[{color}]{session.Duration}min[/]"
                );
                isAlternateRow = !isAlternateRow;
            }
            Console.Clear();
            AnsiConsole.Write(table);
            Console.WriteLine("Press any key to continue...");
            Console.Read();
            Console.Clear();
        }

        public static void CreateTableFiltered(List<SummaryReport> reports, string dateFilter)
        {
            var table = new Table();

            table.BorderColor(Color.DarkSlateGray1);
            table.Border(TableBorder.Rounded);

            table.AddColumn(new TableColumn($"[cyan1]{dateFilter}[/]").LeftAligned());
            table.AddColumn(new TableColumn("[green1]Total Time (min)[/]").RightAligned());
            table.AddColumn(new TableColumn("[green1]Avg Time (min)[/]").RightAligned());
            table.AddColumn(new TableColumn("[blue1]Sessions[/]").RightAligned());
            table.AddColumn(new TableColumn("[magenta1]Activities[/]").LeftAligned());

            bool isAlternateRow = false;
            foreach (var row in reports)
            {
                string formattedActivities = string.Join(", ", row.Activity.Split(','));
                var color = isAlternateRow ? "grey" : "blue";

                table.AddRow(
                    $"[{color}]{row.DateRange}[/]",
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

            table.BorderColor(Color.DarkSlateGray1);
            table.Border(TableBorder.Rounded);

            table.AddColumn(new TableColumn("[cyan1]ID[/]").LeftAligned());
            table.AddColumn(new TableColumn("[green1]Start Date[/]").RightAligned());
            table.AddColumn(new TableColumn("[blue1]End Date[/]").RightAligned());
            table.AddColumn(new TableColumn("[yellow1]Goal Hours[/]").RightAligned());
            table.AddColumn(new TableColumn("[red]Complete?[/]").LeftAligned());

            bool isAlternateRow = false;
            foreach (var goal in codingGoals)
            {
                var color = isAlternateRow ? "grey" : "blue";
                table.AddRow(
                    $"[{color}]{goal.Id.ToString()}[/]",
                    $"[{color}]{goal.GoalStartDate.ToShortDateString()}[/]",
                    $"[{color}]{goal.GoalEndDate.ToShortDateString()}[/]",
                    $"[{color}]{goal.GoalHours.ToString()}[/]",
                    $"[{color}]{goal.Complete.ToString()}[/]"
                );
                isAlternateRow = !isAlternateRow;
            }
            Console.Clear();
            AnsiConsole.Write(table);
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        public static void ShowCodingGoalProgress(CodingGoal goal, SummaryReport sessionData)
        {
            Console.Clear();
            var table = new Table();
            string complete = (sessionData.TotalTime / 60) > (decimal)goal.GoalHours ? "Yeah!" : "Nope";

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
            // table.AddColumn(new TableColumn("[blue1]Target Hours[/]").RightAligned());
            // table.AddColumn(new TableColumn("[yellow1]Actual Hours[/]").RightAligned());
            table.AddColumn(new TableColumn("[red]Complete?[/]").LeftAligned());

            table.AddRow(
                $"{goal.GoalStartDate.ToShortDateString()}",
                $"{goal.GoalEndDate.ToShortDateString()}",
                // $"{goal.GoalHours.ToString()}",
                // $"{(sessionData.TotalTime / 60).ToString()}",
                $"{goal.Complete}"
            );
            AnsiConsole.Write(table);

            double codingTime = Convert.ToDouble(sessionData.TotalTime / 60);
            double goalTime = Convert.ToDouble(goal.GoalHours);

            Console.WriteLine("\n");

            AnsiConsole.Write(new BarChart()
                .Width(75)
                .Label("[green bold underline]Coding Hours[/]")
                .CenterLabel()
                .AddItem("Coding Time", codingTime, Color.Red)
                .AddItem("Goal:", goalTime, Color.Blue));

            Console.WriteLine("\n\n");
        }
        public static void LiveSessionProgress()
        {
            AnsiConsole.Progress()
                // .AutoRefresh(false) // Turn off auto refresh
                // .AutoClear(false)   // Do not remove the task list when done
                // .HideCompleted(false)   // Hide tasks as they are completed
                // .Columns(new ProgressColumn[]
                // {
                //     new TaskDescriptionColumn(),    // Task description
                //     new ProgressBarColumn(),        // Progress bar
                //     new PercentageColumn(),         // Percentage
                //     new RemainingTimeColumn(),      // Remaining time
                //     new SpinnerColumn(),            // Spinner
                // })
                .Start(ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask("[green]Reticulating splines[/]");
                    var task2 = ctx.AddTask("[green]Folding space[/]");

                    while (!ctx.IsFinished)
                    {
                        task1.Increment(1.5);
                        task2.Increment(0.5);

                        Task.Delay(10);
                    }
                });
        }
    }
}