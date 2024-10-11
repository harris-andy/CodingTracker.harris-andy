
# Coding Tracker Project

This is my submission for the cSharpAcademy Habit Logger project found here: [Coding Tracker Project](https://thecsharpacademy.com/project/13/coding-tracker)


## Project Description
- It's a small console CRUD app in which the user can track coding sessions which are stored in a local SQLite database.
- Built with C#/.Net 8, SQLite, Dapper and Spectre Console (that thing is awesome)


## Usage
- Follow the instructions and away you go
- i.e. Select from the menu to perform operations such as: viewing all records, inserting, updating and deleting records.

![main menu](/images/mainmenu2.png)

## Features
- Record a coding session including start date, end date and activity
- Options to add fake data 100 rows at a time and clear the database from main menu
- Record a live coding session with a timer displayed in the console


![stopwatch timer](/images/codingtimer.png)


- Get summary records by day, week or year


![summary year](/images/summary.png)


- Set a coding goal with start and end dates and target hours
- See progress toward your goal - updated with every new coding session entered


![coding goal progress](/images/codingprogress.png)


## More to do
- UI formatting could use some work. That's my Achille's heel.
- Could use better granularity and customization on reports and coding goals. But there's only so much time.
- The DisplayData class, which includes all table/chart printing, needs revision. I tried to condense the functions but I wasn't successful because each table had specific data points and/or structure of source material.


## Questions & Comments
- Note on organization: I made two folders for class files, Classes_Function and Classes_Object. I don't think that's the "correct" way but it works for now.
- I used AI to parse the date in the summary report for weeks. I didn't realize how weird it gets trying to parse dates from week numbers.
- I also used AI for some of the table styling. I can't stand UI design so I gave up and let Claude make it pretty.
- Doing user input verification with Spectre is SO much better - I'm using that for everything now
- I tried to improve my organization but it still needs work. 
- I used "public static" on everything which I feel isn't right. I'd appreciate any advice on this.
- Getting the report summaries was quite a pain at first. I tried using LINQ to manipulate the data but I was working with groups of lists of objects and I gave up. So I switched to better SQL queries and that made it a lot easier to parse the data.
