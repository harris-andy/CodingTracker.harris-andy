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
    }
}