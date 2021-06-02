using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Progbase3ClassLib
{
    public class RemoteService
    {
        public RemoteUsersRepository usersRepo;
        public RemotePostsRepository postsRepo;
        public RemoteCommentsRepository commentsRepo;
        public RemoteService()
        {
            IPAddress ipAddress = IPAddress.Loopback;
            int port = 3000;

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            try
            {
                sender.Connect(remoteEP);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
            usersRepo = new RemoteUsersRepository(sender);
            postsRepo = new RemotePostsRepository(sender);
            commentsRepo = new RemoteCommentsRepository(sender);
        }
    }
}
