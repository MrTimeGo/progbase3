using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            const string databaseFilePath = @".\..\data\database.db";
            ConsoleInterface.Run(databaseFilePath);
        }
    }
}
