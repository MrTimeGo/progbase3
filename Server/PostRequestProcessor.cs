using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Net;
using Progbase3ClassLib;

namespace Server
{
    class PostRequestProcessor
    {
        Socket handler;
        Service service;
        public PostRequestProcessor(Socket handler, Service service)
        {
            this.handler = handler;
            this.service = service;
        }
        public void ProcessRequest(Request request)
        {
            switch (request.methodName)
            {
                case "post.Insert": ProcessInsert(request); break;
                case "post.GetById": ProcessGetById(request); break;
                case "post.Edit": ProcessEdit(request); break;
                case "post.DeleteById": ProcessDeleteById(request); break;
                case "post.GetTotalPages": ProcessGetTotalPages(request); break;
                case "post.GetPage": ProcessGetPage(request); break;
            }
        }
        private void ProcessInsert(Request request)
        {
            Post post = Post.Parse(request.parameters[0]);
            long returnValue = service.postsRepo.Insert(post);
            Response<long> response = new Response<long>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetById(Request request)
        {
            long id = long.Parse(request.parameters[0]);
            Post returnValue = service.postsRepo.GetById(id);
            Response<Post> response = new Response<Post>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessEdit(Request request)
        {
            Post post = Post.Parse(request.parameters[0]);
            int returnValue = service.postsRepo.Edit(post);
            Response<int> response = new Response<int>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessDeleteById(Request request)
        {
            long id = long.Parse(request.parameters[0]);
            int returnValue = service.postsRepo.DeleteById(id);
            Response<int> response = new Response<int>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetTotalPages(Request request)
        {
            string searchKeyword = request.parameters[0];
            long authorId = long.Parse(request.parameters[1]);
            int returnValue = service.postsRepo.GetTotalPages(searchKeyword, authorId);
            Response<int> response = new Response<int>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetPage(Request request)
        {
            int pageNumber = int.Parse(request.parameters[0]);
            string searchKeyword = request.parameters[1];
            long authorId = long.Parse(request.parameters[2]);
            List<Post> returnValue = service.postsRepo.GetPage(pageNumber, searchKeyword, authorId);
            Response<List<Post>> response = new Response<List<Post>>()
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

