using System;
using SearchFile;
using ExceptionsandValidaton;
using DB;

namespace UI_Interface
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            //Declare variables and sign up for events
            SearchClass SearchMethod = new SearchClass();
            SearchMethod.printEvent += PrintResults;
            validationTest validation = new validationTest();//will be used for validate string inputs of both searchString and directory
            validation.ValidationError += ErrorMethod;
            ResultsToDB DataBase = new ResultsToDB(); //set the path to DB at the local computer
            DataBase.DataBaseError += ErrorMethod;
            DataBase.SetDBLocation();            
            //------------------------------------------------
            Console.WriteLine("Welcome to the File Searcher Ido Ben Tora \n");
            while (true)
            {
                OptionsMenu(); //presenting the search options to the user
                char ChosenOption = Console.ReadKey(true).KeyChar;
                switch (ChosenOption)
                {
                    case '1':
                        SearchProcess();
                        break;
                    case '2':
                        Console.WriteLine("Enter Directory name");
                        string SearchDirectory = Console.ReadLine();
                        if (validation.isDirectoryValidAndExist(SearchDirectory))
                        {
                            SearchProcess(SearchDirectory);
                        }
                        break;
                    case '3':
                        Exit();
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Error \nPlease choose 1,2 or 3");
                        Console.ResetColor();
                        break;
                }
            }
            //-----------Class's Methods------------------
            static void OptionsMenu()
            {
                Console.WriteLine("Choose the prefered option:");
                Console.WriteLine("1-Enter file name to search");
                Console.WriteLine("2-Enter file name to search+parent directory to search in");
                Console.WriteLine("3-Exit");
            }//select the value of ChosenOption
            void SearchProcess(string SearchDirectory = "c:\\")//Default value
            {
                Console.Write("File name:");
                SearchMethod.SearchString = Console.ReadLine();
                if (validation.isFileValid(SearchMethod.SearchString))
                {
                    SearchMethod.search(SearchDirectory);//start searching
                    ResponseAfterSearching(SearchMethod.NumOfResults);
                }
            }//activate if the user chose one of the search options
            void ResponseAfterSearching(int Results)
            {
                Console.WriteLine((Results >= 1) ? "Total number of results: " + Results : "No files found!");
                Console.WriteLine("Do you want to keep searching? \nY/N");
                char keepSearch = char.ToUpper(Console.ReadKey(true).KeyChar);
                if (keepSearch == 'N') Exit();
            }
            static void PrintResults(string file)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(file);
                Console.ResetColor();
            }
            static void ErrorMethod(string ErrorMessage)//used for all exceptions
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ErrorMessage);
                Console.ResetColor();
            }
            static void Exit()
            {
                Console.WriteLine("\nThanks for using \"FileSearcher\"");
                Console.WriteLine("The console will be automatically closed in 5 seconds");
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }//terminate the console
        }
    }
}