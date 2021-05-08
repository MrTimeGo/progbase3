using System;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            const string databaseFilePath = @"C:\Users\Artem\Desktop\KPI\progbase3\data\database.db";
            //const string databaseFilePath = @".\..\data\database.db";
            if (args[0] == "generator")
            {
                ConsoleInterface.Run(databaseFilePath);
            }
            else if (args[0] == "interface")
            {
                Terminal.RunInterface(databaseFilePath);
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }
    }
}
