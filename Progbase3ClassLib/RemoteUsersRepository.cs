using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Progbase3ClassLib
{
    public class RemoteUsersRepository
    {
        Socket sender;
        public long Insert(User user)
        {
            throw new NotImplementedException();
        }
        public User GetById(long id)
        {
            throw new NotImplementedException();
        }
        public int Edit(User editedUser)
        {
            throw new NotImplementedException();
        }
        public int DeleteById(long id)
        {
            throw new NotImplementedException();
        }
        public bool UserExists(string username)
        {
            throw new NotImplementedException();
        }
        public User GetByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
