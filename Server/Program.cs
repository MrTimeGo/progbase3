using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

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

                    Thread newClientThread = new Thread(ProcessClient);
                    newClientThread.Start(handler);
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void ProcessClient(object obj)
        {
            Socket newClient = (Socket)obj;

            byte[] bytes = new byte[1024];  // buffer
            string data = "";
            while (true)
            {
                int bytesRec = newClient.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }
            
        }
    }
}
