using System;
using Storage;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            const string databaseFilePath = @".\..\data\database.db";
            Service service = new Service(databaseFilePath);
            Server server = new Server(service);
            server.RunServer();
        }
    }
}
