using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker.harris_andy
{
    public class UserInput
    {
        public static void MainMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine(
                    "--------------------------------------------------\n" +
                    "\n\t\tMAIN MENU\n\n" +
                    "\tWhat would you like to do?\n\n" +
                    "\tType 0 to Close Application\n" +
                    "\tType 1 to View All Records\n" +
                    "\tType 2 to Insert Record\n" +
                    "\tType 3 to Delete Record\n" +
                    "\tType 4 to Update Record\n" +
                    "\tType 5 to View A Record Summary\n" +
                    "\tType 6 to Delete All Records :(\n" +
                    "\tType 7 to Add 100 Rows of Fake Data\n" +
                    "\tType 8 to Start a Timed Coding Session. Neat!\n" +
                    "--------------------------------------------------\n");

                int inputNumber = GetMenuChoice(0, 8);

                switch (inputNumber)
                {
                    case 0:
                        Console.WriteLine("\nBye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case 1:
                        int answer = GetAllOrFiltered();
                        if (answer == 1)
                        {
                            List<CodingSession> sessions = RetrieveRecord.GetRecords();
                            DisplayData.CreateTable(sessions);
                        }
                        else if (answer == 2) RetrieveRecord.GetFilteredRecords();
                        break;
                    case 2:
                        // (DateTime startDateTime, DateTime endDateTime, string activity) = GetSessionData();
                        CodingSession session = GetSessionData();
                        DBInteractions.Insert(session);
                        break;
                    case 3:
                        DBInteractions.Delete();
                        break;
                    case 4:
                        DBInteractions.Update();
                        break;
                    case 5:
                        // RetrieveRecord.GetRecordSummary();
                        break;
                    case 6:
                        DBInteractions.DeleteTableContents();
                        break;
                    case 7:
                        DBInteractions.PopulateDatabase();
                        break;
                    case 8:
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\nInvalid Command. Give me number!");
                        break;
                }
            }
        }

        // public static (DateTime startDateTime, DateTime endDateTime, string activity) GetSessionData()
        public static CodingSession GetSessionData()
        {
            Console.Clear();
            DateTime date = GetDate();
            Console.Clear();
            TimeSpan startTime = GetTime("start");
            Console.Clear();
            TimeSpan endTime = GetTime("end");
            Console.Clear();
            string activity = GetActivity();
            (DateTime startDateTime, DateTime endDateTime) = ParseDateTimes(date, startTime, endTime);

            CodingSession session = new CodingSession
            {
                StartDayTime = startDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDayTime = endDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Activity = activity
            };
            // return (startDateTime, endDateTime, activity);
            return session;
        }

        public static int GetMenuChoice(int start, int end)
        {
            var menuChoice = AnsiConsole.Prompt(
            new TextPrompt<int>("Menu choice:")
            .Validate((n) =>
            {
                if (start <= n && n <= end)
                    return ValidationResult.Success();

                else
                    return ValidationResult.Error($"[red]Pick a valid option[/]");
            }));
            return menuChoice;
        }

        public static (int, bool) GetRecordID(string option)
        {
            List<CodingSession> records = RetrieveRecord.GetRecords();
            DisplayData.CreateTable(records);
            var validIDs = records.Select(r => r.Id).ToList();

            var recordID = AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter ID of record you want to {option}:")
            .Validate((n) =>
            {
                if (validIDs.Contains(n))
                    return ValidationResult.Success();

                else
                    return ValidationResult.Error($"[red]Must be a valid ID[/]");
            }));

            bool confirmation = AnsiConsole.Prompt(
            new TextPrompt<bool>($"Are you sure [red]{recordID}[/] is the record you want to [red]{option}[/]?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n"));

            if (!confirmation)
            {
                Console.WriteLine("");
                Console.WriteLine($"Skipping {option} for now. Better to think about it.");
                Console.WriteLine("");
                Thread.Sleep(1500);
            }
            return (recordID, confirmation);
        }

        public static DateTime GetDate()
        {
            DateTime date = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter session date:"));
            return date;
        }

        public static TimeSpan GetTime(string sessionTime)
        {
            TimeSpan time = AnsiConsole.Prompt(
                new TextPrompt<TimeSpan>($"Enter {sessionTime} time:"));
            return time;
        }

        public static string GetActivity()
        {
            string activity = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your coding activity:")
            );
            return activity;
        }

        public static (DateTime, DateTime) ParseDateTimes(DateTime date, TimeSpan start, TimeSpan end)
        {
            DateTime startDate = date.Date.Add(start);
            DateTime endDate = date.Date.Add(end);
            if (endDate < startDate)
            {
                endDate = endDate.AddDays(1);
            }
            return (startDate, endDate);
        }

        // public static void CreateTable(List<CodingSession> sessions)
        // {
        //     var table = new Table();

        //     table.BorderColor(Color.DarkSlateGray1);
        //     table.Border(TableBorder.Rounded);

        //     table.AddColumn(new TableColumn("[cyan1]ID[/]").LeftAligned());
        //     table.AddColumn(new TableColumn("[blue1]Activity[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[green1]Start Day[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[green1]Start Time[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[red1]End Day[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[red1]End Time[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[yellow1]Duration (min)[/]").LeftAligned());

        //     bool isAlternateRow = false;
        //     foreach (var session in sessions)
        //     {
        //         var color = isAlternateRow ? "grey" : "blue";
        //         table.AddRow(
        //             $"[{color}]{session.Id.ToString()}[/]",
        //             $"[{color}]{session.Activity ?? "N/A"}[/]",
        //             $"[{color}]{session.StartDateTime.ToShortDateString()}[/]",
        //             $"[{color}]{session.StartDateTime.ToShortTimeString()}[/]",
        //             $"[{color}]{session.EndDateTime.ToShortDateString()}[/]",
        //             $"[{color}]{session.EndDateTime.ToShortTimeString()}[/]",
        //             $"[{color}]{session.Duration}min[/]"
        //         );
        //         isAlternateRow = !isAlternateRow;
        //     }
        //     Console.Clear();
        //     AnsiConsole.Write(table);
        //     Console.WriteLine("Press any key to continue...");
        //     Console.Read();
        // }

        // public static void CreateTableFiltered(List<SummaryReport> reports, string dateFilter)
        // {
        //     var table = new Table();

        //     table.BorderColor(Color.DarkSlateGray1);
        //     table.Border(TableBorder.Rounded);

        //     table.AddColumn(new TableColumn($"[cyan1]{dateFilter}[/]").LeftAligned());
        //     table.AddColumn(new TableColumn("[green1]Total Time (min)[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[green1]Avg Time (min)[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[blue1]Sessions[/]").RightAligned());
        //     table.AddColumn(new TableColumn("[magenta1]Activities[/]").LeftAligned());

        //     bool isAlternateRow = false;
        //     foreach (var row in reports)
        //     {
        //         string formattedActivities = string.Join(", ", row.Activity.Split(','));
        //         var color = isAlternateRow ? "grey" : "blue";

        //         table.AddRow(
        //             $"[{color}]{row.DateRange}[/]",
        //             $"[{color}]{row.TotalTime:F1}[/]",
        //             $"[{color}]{row.AvgTime:F1}[/]",
        //             $"[{color}]{row.Sessions}[/]",
        //             $"[{color}]{formattedActivities}[/]"
        //         );
        //         isAlternateRow = !isAlternateRow;
        //     }
        //     AnsiConsole.Write(table);
        //     Console.WriteLine("Press any key to continue...");
        //     Console.Read();
        // }

        public static int GetAllOrFiltered()
        {
            int answer = AnsiConsole.Prompt(
            new TextPrompt<int>("1. All Records\n2. Filtered Records\nEnter choice:")
                .Validate((n) =>
                {
                    if (n == 1 || n == 2)
                        return ValidationResult.Success();
                    else
                        return ValidationResult.Error("[red]Invalid number[/]");
                }));
            Console.Clear();
            return answer;
        }

        public static string FilteredOptionsMenu()
        {
            Console.WriteLine(
                    "--------------------------------------------------\n" +
                    "\tHow would you like your coding session summary?\n\n" +
                    "\tType 0 to Back to Main Menu\n" +
                    "\tType 1 to View Sessions by Day\n" +
                    "\tType 2 to View Sessions by Week\n" +
                    "\tType 3 to View Sessions by Year\n" +
                    "\tType 4 to Select Specific Date Range\n" +
                    "--------------------------------------------------\n");

            int inputNumber = GetMenuChoice(0, 4);
            string filterOption = "";

            switch (inputNumber)
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    filterOption = "Day";
                    break;
                case 2:
                    filterOption = "Week";
                    break;
                case 3:
                    filterOption = "Year";
                    break;
                case 4:
                    filterOption = "dateRange";
                    break;
                default:
                    MainMenu();
                    break;
            }
            return filterOption;
        }
    }
}