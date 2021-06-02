using System;

namespace DataGenerator
{
    class Program
    {
        static void Main()
        {
            const string databaseFilePath = @".\..\data\database.db";
            ConsoleInterface.Run(databaseFilePath);
        }
    }
}
