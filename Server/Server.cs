using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Storage;
using RPC;

namespace Server
{
    class Server
    {
        Service service;
        public Server(Service service)
        {
            this.service = service;
        }
        public void RunServer()
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
        private void ProcessClient(object obj)
        {
            Socket handler = (Socket)obj;
            Console.WriteLine($"{handler.RemoteEndPoint} connected. Waiting for requests..");
            UserRequestProcessor uProcessor = new UserRequestProcessor(handler, service);
            PostRequestProcessor pProcessor = new PostRequestProcessor(handler, service);
            CommentRequestProcessor cProcessor = new CommentRequestProcessor(handler, service);
            try
            {
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    string xmlRequest = "";
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        xmlRequest += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        if (xmlRequest.IndexOf("</request>") > -1)
                        {
                            break;
                        }
                    }

                    Request request = Serializer.DeserializeRequest(xmlRequest);
                    Console.WriteLine($"Get request from {handler.RemoteEndPoint}: {request.methodName}");
                    try
                    {
                        if (request.methodName.StartsWith("user."))
                        {
                            uProcessor.ProcessRequest(request);
                        }
                        else if (request.methodName.StartsWith("post."))
                        {
                            pProcessor.ProcessRequest(request);
                        }
                        else if (request.methodName.StartsWith("comment."))
                        {
                            cProcessor.ProcessRequest(request);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error while connected to data base");
                        handler.Disconnect(false);
                        Console.WriteLine($"Client {handler.RemoteEndPoint} was disconected");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhadled exception: {ex.Message}");
                Console.WriteLine($"Client {handler.RemoteEndPoint} was disconected");
            }
        }
    }
}
