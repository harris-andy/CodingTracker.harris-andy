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
                        RetrieveRecord.GetRecords();
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
            var validIDs = records.Select(r => r.Id).ToList();

            var recordID = AnsiConsole.Prompt(
            new TextPrompt<int>($"Which record do you want to {option}:")
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

        public static void CreateTable(List<CodingSession> sessions)
        {
            // StartDateTime, EndDateTime, Activity, Duration, Id
            // PASS IN LIST OF COLUMN NAMES TO MAKE COLUMNS DYNAMIC
            var table = new Table();
            table.AddColumn("ID").Centered();
            table.AddColumn("Activity").Centered();
            table.AddColumn("Start Day").Centered();
            table.AddColumn("Start Time").Centered();
            table.AddColumn("End Day").Centered();
            table.AddColumn("End Time").Centered();
            table.AddColumn("Duration").Centered();

            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.Activity ?? "N/A",
                    session.StartDateTime.ToShortDateString(),
                    session.StartDateTime.ToShortTimeString(),
                    session.EndDateTime.ToShortDateString(),
                    session.EndDateTime.ToShortTimeString(),
                    $"{session.Duration} min"
                );
            }
            Console.Clear();
            AnsiConsole.Write(table);
        }

        public static string GetAllOrFiltered()
        {
            var answer = AnsiConsole.Prompt(
            new TextPrompt<string>("All records or filtered?")
                .AddChoices(["All", "Filtered"]));
            return answer;
        }

        public static string FilteredOptionsMenu()
        {
            Console.WriteLine(
                    "--------------------------------------------------\n" +
                    // "\n\t\tMAIN MENU\n\n" +
                    "\tHow would you like your coding session summary?\n\n" +
                    "\tType 0 to Back to Main Menu\n" +
                    "\tType 1 to View Sessions by Day\n" +
                    "\tType 2 to View Sessions by Week\n" +
                    "\tType 3 to View Sessions by Year\n" +
                    "\tType 4 to Select Specific Date Range\n" +
                    // "\tType 5 to View A Record Summary\n" +
                    // "\tType 6 to Delete All Records :(\n" +
                    // "\tType 7 to Add 100 Rows of Fake Data\n" +
                    // "\tType 8 to Start a Timed Coding Session. Neat!\n" +
                    "--------------------------------------------------\n");

            int inputNumber = GetMenuChoice(0, 4);
            string filterOption = "";

            switch (inputNumber)
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    filterOption = "day";
                    break;
                case 2:
                    filterOption = "week";
                    break;
                case 3:
                    filterOption = "year";
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