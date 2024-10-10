/*
Requirements

- Fix filtered week output to use Week Starting

- Clean up Create Tables functions - there's a lot of repetition

*/
using CodingTracker.harris_andy;


internal class Program
{
    private static void Main(string[] args)
    {
        DBInteractions.InitializeDatabase();
        DBInteractions.InitializeCodingGoalDatabase();

        UserInput.MainMenu();
    }
}