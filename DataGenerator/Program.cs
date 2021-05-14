using System;

namespace DataGenerator
{
    class Program
    {
        static void Main()
        {
            const string databaseFilePath = @"C:\Users\Artem\Desktop\KPI\progbase3\data\database.db";
            ConsoleInterface.Run(databaseFilePath);
        }
    }
}
