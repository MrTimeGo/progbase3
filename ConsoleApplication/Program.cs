using System;

namespace DataManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Terminal gui = new Terminal();
            try
            {
                gui.RunInterface();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception: {ex.Message}");
                Console.WriteLine($"Disconnected from a server");
            }
        }
    }
}
