using System;
using static System.Console;
using Microsoft.Data.Sqlite;

namespace DataGenerator
{
    static class ConsoleInterface
    {
        public static void Run(string databaseFilepath)
        {
            bool exit = false;
            SqliteConnection connection = new SqliteConnection($"Data Source = {databaseFilepath}");
            while (!exit)
            {
                try
                {
                    string input = ReadInput();

                    Arguments args = ParseInput(input);

                    if (args.command == "exit")
                    {
                        exit = true;
                    }
                    else if (args.command == "help")
                    {
                        WriteLine(GetHelpString());
                    }
                    else if (args.command == "generate")
                    {
                        ProcessGenerate(args, connection);
                    }
                }
                catch (Exception ex)
                {
                    Error.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        private static void ProcessGenerate(Arguments args, SqliteConnection connection)
        {
            if (args.otherArguments.Length != 4)
            {
                throw new ArgumentException($"Incorrent parameters length. Should be {4}, got: {args.otherArguments.Length}");
            }
            string entity = args.otherArguments[0];
            ValidateEntity(entity);

            if (!int.TryParse(args.otherArguments[1], out int number))
            {
                throw new ArgumentException($"Second parameter should be integer. Got: {args.otherArguments[1]}");
            }

            if (!DateTime.TryParse(args.otherArguments[2], out DateTime dateFrom))
            {
                throw new ArgumentException($"Third parameter should be date time string. Got: {args.otherArguments[2]}");
            }

            if (!DateTime.TryParse(args.otherArguments[3], out DateTime dateTo))
            {
                throw new ArgumentException($"Fourth parameter should be date time string. Got: {args.otherArguments[3]}");
            }

            if (entity == "user")
            {
                DataGenerator.GenerateUsers(connection, number, dateFrom, dateTo);
            }
            else if (entity == "post")
            {
                DataGenerator.GeneratePosts(connection, number, dateFrom, dateTo);
            }
            else if (entity == "comment")
            {
                DataGenerator.GenerateComments(connection, number, dateFrom, dateTo);
            }
        }
        private static void ValidateEntity(string entity)
        {
            string[] validEntities = new string[] { "user", "post", "comment" };
            foreach(string validEntity in validEntities)
            {
                if (entity == validEntity)
                {
                    return;
                }
            }
            throw new ArgumentException($"Incorrect entity.");
        }
        private static string GetHelpString()
        {
            string[] commands = new string[] { "generate {entity} {amount} {date from} {date to}", "exit"};
            string result = "";
            foreach (string command in commands)
            {
                result += command + "\n";
            }
            return result;
        }
        struct Arguments
        {
            public string command;
            public string[] otherArguments;
        }
        private static Arguments ParseInput(string input)
        {
            string[] subcommands = input.Trim().Split(' ');

            ValidateCommandLength(subcommands.Length);
            string command = subcommands[0];
            ValidateCommand(command);

            string[] otherArguments = new string[subcommands.Length - 1];
            for (int i = 0; i < otherArguments.Length; i++)
            {
                otherArguments[i] = subcommands[i + 1];
            }
            return new Arguments()
            { 
                command = command,
                otherArguments = otherArguments
            };
        }
        private static void ValidateCommand(string command)
        {
            string[] validCommands = new string[] { "generate", "exit", "help" };
            foreach(string validCommand in validCommands)
            {
                if (command == validCommand)
                {
                    return;
                }
            }
            throw new ArgumentException($"Command {command} does not exists.");
        }
        private static void ValidateCommandLength(int length)
        {
            if (length < 1)
            {
                throw new ArgumentException($"Argument length should be more than 0. Got: {length}");
            }
        }
        private static string ReadInput()
        {
            Write("> ");
            return ReadLine();
        }
    }
}
