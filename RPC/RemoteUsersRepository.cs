using System;
using System.Collections.Generic;
using Progbase3ClassLib;
using System.Net.Sockets;
using System.Text;

namespace RPC
{
    public class RemoteUsersRepository
    {
        Socket sender;
        public RemoteUsersRepository(Socket sender)
        {
            this.sender = sender;
        }
        public long Insert(User user)
        {
            List<string> parameters = new List<string>();
            parameters.Add(user.GetStringRepresentation());
            Request request = new Request()
            {
                methodName = "user.Insert",
                parameters = parameters
            };

            SendRequest(request);
            Response<long> response = GetResponse<long>();

            return response.returnValue;
        }
        public User GetById(long id)
        {
            List<string> parameters = new List<string>();
            parameters.Add(id.ToString());
            Request request = new Request()
            {
                methodName = "user.GetById",
                parameters = parameters
            };

            SendRequest(request);
            Response<User> response = GetResponse<User>();

            return response.returnValue;
        }
        public int Edit(User editedUser)
        {
            List<string> parameters = new List<string>();
            parameters.Add(editedUser.GetStringRepresentation());
            Request request = new Request()
            {
                methodName = "user.Edit",
                parameters = parameters
            };

            SendRequest(request);
            Response<int> response = GetResponse<int>();

            return response.returnValue;
        }
        public int DeleteById(long id)
        {
            List<string> parameters = new List<string>();
            parameters.Add(id.ToString());
            Request request = new Request()
            {
                methodName = "user.DeleteById",
                parameters = parameters
            };

            SendRequest(request);
            Response<int> response = GetResponse<int>();

            return response.returnValue;
        }
        public bool UserExists(string username)
        {
            List<string> parameters = new List<string>();
            parameters.Add(username);
            Request request = new Request()
            {
                methodName = "user.UserExists",
                parameters = parameters
            };

            SendRequest(request);
            Response<bool> response = GetResponse<bool>();

            return response.returnValue;
        }
        public User GetByUsername(string username)
        {
            List<string> parameters = new List<string>();
            parameters.Add(username);
            Request request = new Request()
            {
                methodName = "user.GetByUsername",
                parameters = parameters
            };

            SendRequest(request);
            Response<User> response = GetResponse<User>();

            return response.returnValue;
        }
        private void SendRequest(Request request)
        {
            string xmlRequest = Serializer.SerializeRequest(request);
            byte[] msg = Encoding.UTF8.GetBytes(xmlRequest);

            sender.Send(msg);
        }
        private Response<T> GetResponse<T>()
        {
            byte[] bytes = new byte[1024];  // buffer
            string xmlResponse = "";
            while (true)
            {
                int bytesRec = sender.Receive(bytes);
                xmlResponse += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                if (xmlResponse.IndexOf("</response>") > -1 || xmlResponse == "")
                {
                    break;
                }
            }

            if (xmlResponse == "")
            {
                throw new Exception("Server error");
            }
            Response<T> response = Serializer.DeserializeResponse<T>(xmlResponse);
            return response;
        }
    }
}
