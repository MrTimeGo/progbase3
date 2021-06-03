using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Storage;
using System.Text;

namespace RPC
{
    public class UserRequestProcessor
    {
        Socket handler;
        Service service;
        public UserRequestProcessor(Socket handler, Service service)
        {
            this.handler = handler;
            this.service = service;
        }
        public void ProcessRequest(Request request)
        {
            switch (request.methodName)
            {
                case "user.Insert": ProcessInsert(request); break;
                case "user.GetById": ProcessGetById(request); break;
                case "user.Edit": ProcessEdit(request); break;
                case "user.DeleteById": ProcessDeleteById(request); break;
                case "user.UserExists": ProcessUserExists(request); break;
                case "user.GetByUsername": ProcessGetByUsername(request); break;
            }
        }
        private void ProcessInsert(Request request)
        {
            User user = User.Parse(request.parameters[0]);
            long returnValue = service.usersRepo.Insert(user);
            Response<long> response = new Response<long>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetById(Request request)
        {
            long id = long.Parse(request.parameters[0]);
            User returnValue = service.usersRepo.GetById(id);
            Response<User> response = new Response<User>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessEdit(Request request)
        {
            User editedUser = User.Parse(request.parameters[0]);
            int returnValue = service.usersRepo.Edit(editedUser);
            Response<int> response = new Response<int>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessDeleteById(Request request)
        {
            long id = long.Parse(request.parameters[0]);
            int returnValue = service.usersRepo.DeleteById(id);
            Response<int> response = new Response<int>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessUserExists(Request request)
        {
            string username = request.parameters[0];
            bool returnValue = service.usersRepo.UserExists(username);
            Response<bool> response = new Response<bool>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetByUsername(Request request)
        {
            string username = request.parameters[0];
            User returnValue = service.usersRepo.GetByUsername(username);
            Response<User> response = new Response<User>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void SendResponse<T>(Response<T> response)
        {
            string xmlResponse = Serializer.SerializeResponse(response);
            byte[] msg = Encoding.UTF8.GetBytes(xmlResponse);
            handler.Send(msg);
            Console.WriteLine($"Response to {handler.RemoteEndPoint} was sent");
        }
    }
}
