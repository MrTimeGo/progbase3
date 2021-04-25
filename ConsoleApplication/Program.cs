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
            if (args.Length != 2)
            {
                Console.WriteLine("Agrugument should be 2");
                return;
            }

            if (!int.TryParse(args[1], out int n))
            {
                Console.WriteLine("Second argument should be integer");
                return;
            }

            if (args[0] == "user")
            {
                DataGenerator.GenerateUsers(n, databaseFilePath);
                Console.WriteLine($"Succesfully generated {n} users.");
            }
            else if (args[0] == "post")
            {
                DataGenerator.GeneratePosts(n, databaseFilePath);
                Console.WriteLine($"Succesfully generated {n} posts.");
            }
            else if (args[0] == "comment")
            {
                DataGenerator.GenerateComments(n, databaseFilePath);
                Console.WriteLine($"Succesfully generated {n} comments.");
            }
            else
            {
                Console.WriteLine("First argument should be 'user', 'post' or 'comment'.");
            }

        }
    }
}
