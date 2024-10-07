using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker.harris_andy
{
    public class Spectre
    {
        public static int GetMenuChoice()
        {
            // var name = AnsiConsole.Prompt(
            //     new TextPrompt<string>("What's your name?"));
            // var age = AnsiConsole.Prompt(
            //     new TextPrompt<int>("What's your age?"));

            // AnsiConsole.Write(new Markup("[red]Invalid Number. Must be 0-7[/]"));

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

        public static void CreateTable(List<CodingSession> sessions)
        {
            // StartDateTime, EndDateTime, Activity, Duration, Id
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