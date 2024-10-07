using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class UserInput
    {

        public void MainMenu()
        {
            Console.Clear();
            bool closeApp = false;
            bool withIds = false;
            while (closeApp == false)
            {
                string message =
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
                    "--------------------------------------------------\n";

                int inputNumber = -1;
                while (inputNumber < 0)
                {
                    inputNumber = validateNumberEntry(message, isMainMenu: true);
                    Console.Clear();
                }

                switch (inputNumber)
                {
                    case 0:
                        Console.WriteLine("\nBye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case 1:
                        RetrieveRecord.GetAllRecords(withIds);
                        break;
                    case 2:
                        DBInteractions.Insert();
                        break;
                    case 3:
                        DBInteractions.Delete();
                        break;
                    case 4:
                        DBInteractions.Update();
                        break;
                    case 5:
                        RetrieveRecord.GetRecordSummary();
                        break;
                    case 6:
                        DBInteractions.DeleteTableContents();
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

        // public string GetDateInput()
        // {
        // string? date = null;
        // while (!DateTime.TryParseExact(date, format: "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        // {
        //     Console.WriteLine("Enter date in format dd-mm-yyyy. Press 0 to return to Main Menu");
        //     date = Console.ReadLine();
        //     if (int.TryParse(date, out int number))
        //     {
        //         if (number == 0) MainMenu();
        //     }
        // }
        // return date;
        // }

        // public string GetProgrammingActivity()
        // {
        // string? hobby = "";
        // while (string.IsNullOrWhiteSpace(hobby))
        // {
        //     Console.WriteLine("Enter name of activity (keep it short). Or press 0 to return to Main Menu");
        //     string? temp = Console.ReadLine();
        //     hobby = temp?.Trim().ToLower();
        // }
        // if (hobby == "0") MainMenu();
        // return hobby;
        // }

    }
}