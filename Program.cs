/*
Requirements

- Fix filtered week output to use Week Starting

- Clean up Create Tables functions - there's a lot of repetition

Challenges

    Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.

*/

using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;
using CodingTracker.harris_andy;


internal class Program
{
    private static void Main(string[] args)
    {
        DBInteractions.InitializeDatabase();
        DBInteractions.InitializeCodingGoalDatabase();
        // DisplayData.LiveSessionProgress();

        UserInput.MainMenu();
    }
}