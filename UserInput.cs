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
                    "--------------------------------------------------\n");

                int inputNumber = GetMenuChoice();

                switch (inputNumber)
                {
                    case 0:
                        Console.WriteLine("\nBye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case 1:
                        RetrieveRecord.GetAllRecords();
                        break;
                    case 2:
                        GetSessionData();
                        break;
                    case 3:
                        DBInteractions.Delete();
                        break;
                    case 4:
                        // DBInteractions.Update();
                        break;
                    case 5:
                        // RetrieveRecord.GetRecordSummary();
                        break;
                    case 6:
                        // DBInteractions.DeleteTableContents();
                        break;
                    case 7:
                        DBInteractions.PopulateDatabase();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\nInvalid Command. Give me number!");
                        break;
                }
            }
        }

        public static void GetSessionData()
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

            DBInteractions.Insert(startDateTime, endDateTime, activity);
        }

        public static int GetMenuChoice()
        {
            var menuChoice = AnsiConsole.Prompt(
            new TextPrompt<int>("Menu choice:")
            .Validate((n) => n switch
            {
                < 0 => ValidationResult.Error("[red]Invalid number. Must be 0-7[/]"),
                > 7 => ValidationResult.Error("[red]Invalid number. Must be 0-7[/]"),
                _ => ValidationResult.Success(),
            }));
            return menuChoice;
        }

        public static (int, bool) GetRecordID(string option)
        {
            List<CodingSession> records = RetrieveRecord.GetAllRecords();
            var recordID = AnsiConsole.Prompt(
            new TextPrompt<int>($"Which record do you want to {option}:")
            .Validate((n) =>
            {
                if (n < 1 || n > records.Count)
                {
                    return ValidationResult.Error($"Record ID must be in the range 1-{records.Count}");
                }
                else
                {
                    return ValidationResult.Success();
                }
            }));

            bool confirmation = AnsiConsole.Prompt(
            new TextPrompt<bool>($"Are you sure [red]{recordID}[/] is the record you want to [red]{option}[/]?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n"));

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

    }
}