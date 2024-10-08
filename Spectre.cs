using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker.harris_andy
{
    public class Spectre
    {
        public static int GetMenuChoice()
        {
            var number = AnsiConsole.Prompt(
            new TextPrompt<int>("Menu choice:")
            .Validate((n) => n switch
            {
                < 0 => ValidationResult.Error("[red]Invalid number. Must be 0-7[/]"),
                > 7 => ValidationResult.Error("[red]Invalid number. Must be 0-7[/]"),
                _ => ValidationResult.Success(),
            }));

            return number;
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
            DateTime startDate = date.Add(start);
            DateTime endDate = startDate.Add(end);
            return (startDate, endDate);
        }

        public static void CreateTable(List<CodingSession> sessions)
        {
            // StartDateTime, EndDateTime, Activity, Duration, Id
            // MAY HAVE TO EDIT THIS TO BE DYNAMIC
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