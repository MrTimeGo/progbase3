using System;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            const string databaseFilePath = @"C:\Users\Artem\Desktop\KPI\progbase3\data\database.db";
            //const string databaseFilePath = @".\..\data\database.db";
            Terminal.RunInterface(databaseFilePath);
        }
    }
}
