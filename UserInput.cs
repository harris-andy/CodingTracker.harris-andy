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
                    "\tType 2 to View Records by Date Range\n" +
                    "\tType 3 to Insert Record\n" +
                    "\tType 4 to Delete Record\n" +
                    "\tType 5 to Update Record\n" +
                    "\tType 6 to Delete All Records :(\n" +
                    "\tType 7 to Add 100 Rows of Fake Data\n" +
                    "\tType 8 to Start a Timed Coding Session. Neat!\n" +
                    "\tType 9 to Set a Coding Goal\n" +
                    "\tType 10 to Get Coding Goal Progress\n" +
                    "--------------------------------------------------\n");

                int inputNumber = GetMenuChoice(0, 10);

                switch (inputNumber)
                {
                    case 0:
                        Console.WriteLine("\nBye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case 1:
                        List<CodingSession> sessions = RetrieveRecord.GetRecords();
                        DisplayData.CreateTable(sessions);
                        break;
                    case 2:
                        RetrieveRecord.GetFilteredRecords();
                        break;
                    case 3:
                        CodingSession session = GetSessionData();
                        DBInteractions.Insert(session);
                        break;
                    case 4:
                        DBInteractions.Delete();
                        break;
                    case 5:
                        DBInteractions.Update();
                        break;
                    case 6:
                        DBInteractions.DeleteTableContents();
                        break;
                    case 7:
                        DBInteractions.PopulateDatabase();
                        break;
                    case 8:
                        DisplayData.LiveSessionProgress();
                        break;
                    case 9:
                        CodingGoal goal = SetCodingGoal();
                        DBInteractions.InsertCodingGoal(goal);
                        break;
                    case 10:
                        RetrieveRecord.GetCodingGoals();
                        // Create Table with codingGoal
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\nInvalid Command. Give me number!");
                        break;
                }
            }
        }

        public static CodingSession GetSessionData()
        {
            Console.Clear();
            DateTime date = GetDate("session");
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

        public static DateTime GetDate(string option)
        {
            DateTime date = AnsiConsole.Prompt(
                new TextPrompt<DateTime>($"Enter {option} date:"));
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

        // public static int GetAllOrFiltered()
        // {
        //     int answer = AnsiConsole.Prompt(
        //     new TextPrompt<int>("1. All Records\n2. Filtered Records\nEnter choice:")
        //         .Validate((n) =>
        //         {
        //             if (n == 1 || n == 2)
        //                 return ValidationResult.Success();
        //             else
        //                 return ValidationResult.Error("[red]Invalid number[/]");
        //         }));
        //     Console.Clear();
        //     return answer;
        // }

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

        public static CodingGoal SetCodingGoal()
        {
            // int goalTimeForm = AnsiConsole.Prompt(
            // new TextPrompt<int>("Set Coding Goal Date Range:\n1. Day\n2. Week\n3. Year\nEnter choice:")
            //     .Validate((n) =>
            //     {
            //         if (n <= 3 && n >= 1)
            //             return ValidationResult.Success();
            //         else
            //             return ValidationResult.Error("[red]Invalid number[/]");
            //     }));

            // string goalTimeForm = AnsiConsole.Prompt(
            //     new TextPrompt<string>("Set your coding goal time format:")
            //     .AddChoices(["Day", "Week", "Year"]));

            DateTime startDate = GetDate("coding goal start");
            DateTime userInputDate = GetDate("coding goal end");
            DateTime endDate = userInputDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            string complete = "no";

            float goalHours = AnsiConsole.Prompt(
            new TextPrompt<float>("What's your coding goal?\nEnter hours:")
                .Validate((n) =>
                {
                    if (n >= 0)
                        return ValidationResult.Success();
                    else
                        return ValidationResult.Error("[red]Invalid number[/]");
                }));

            CodingGoal goal = new CodingGoal(startDate, endDate, goalHours, complete);

            return goal;
        }
    }
}