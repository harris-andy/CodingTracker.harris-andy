﻿/*
Requirements

This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.

Challenges

    Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.

    Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.

    Create reports where the users can see their total and average coding session per period.

    Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.
*/

using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;
using CodingTracker.harris_andy;


DBInteractions.InitializeDatabase();

UserInput.MainMenu();
