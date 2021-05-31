using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServer();
        }
        static void RunServer()
        {
            IPAddress ipAddress = IPAddress.Loopback;
            int port = 3000;

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            try
            {
                listener.Bind(localEndPoint);

                listener.Listen();

                while (true)
                {
                    Console.WriteLine($"Waiting for a connection on port {port}");
                    Socket handler = listener.Accept();

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void ProcessClient(Socket newClientSocket)
        {

        }
    }
}
