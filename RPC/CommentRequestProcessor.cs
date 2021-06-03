using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Storage;

namespace RPC
{
    public class CommentRequestProcessor
    {
        Socket handler;
        Service service;
        public CommentRequestProcessor(Socket handler, Service service)
        {
            this.handler = handler;
            this.service = service;
        }
        public void ProcessRequest(Request request)
        {
            switch (request.methodName)
            {
                case "comment.Insert": ProcessInsert(request); break;
                case "comment.GetById": ProcessGetById(request); break;
                case "comment.EditById": ProcessEditById(request); break;
                case "comment.DeleteById": ProcessDeleteById(request); break;
                case "comment.GetByPostId": ProcessGetByPostId(request); break;
                case "comment.GetTotalPages": ProcessGetTotalPages(request); break;
                case "comment.GetPage": ProcessGetPage(request); break;
                case "comment.GetPinnedComment": ProcessGetPinnedComment(request); break;
                case "comment.GetCommentCountBasedOnTimeSpan": ProcessGetCommentCountBasedOnTimeSpan(request); break;
            }
        }
        private void ProcessInsert(Request request)
        {
            Comment comment = Comment.Parse(request.parameters[0]);
            long returnValue = service.commentsRepo.Insert(comment);
            Response<long> response = new Response<long>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetById(Request request)
        {
            long id = long.Parse(request.parameters[0]);
            Comment returnValue = service.commentsRepo.GetById(id);
            Response<Comment> response = new Response<Comment>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessEditById(Request request)
        {
            Comment comment = Comment.Parse(request.parameters[0]);
            int returnValue = service.commentsRepo.EditById(comment);
            Response<int> response = new Response<int>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessDeleteById(Request request)
        {
            long id = long.Parse(request.parameters[0]);
            int returnValue = service.commentsRepo.DeleteById(id);
            Response<int> response = new Response<int>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetByPostId(Request request)
        {
            long postId = long.Parse(request.parameters[0]);
            List<Comment> returnValue = service.commentsRepo.GetByPostId(postId);
            Response<List<Comment>> response = new Response<List<Comment>>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetTotalPages(Request request)
        {
            string searchKeyword = request.parameters[0];
            long id = long.Parse(request.parameters[1]);
            bool isAuthor = bool.Parse(request.parameters[2]);
            int returnValue = service.commentsRepo.GetTotalPages(searchKeyword, id, isAuthor);
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
            long id = long.Parse(request.parameters[2]);
            bool isAuthor = bool.Parse(request.parameters[3]);
            List<Comment> returnValue = service.commentsRepo.GetPage(pageNumber, searchKeyword, id, isAuthor);
            Response<List<Comment>> response = new Response<List<Comment>>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetPinnedComment(Request request)
        {
            long postId = long.Parse(request.parameters[0]);
            Comment returnValue = service.commentsRepo.GetPinnedComment(postId);
            Response<Comment> response = new Response<Comment>()
            {
                returnValue = returnValue
            };

            SendResponse(response);
        }
        private void ProcessGetCommentCountBasedOnTimeSpan(Request request)
        {
            long postId = long.Parse(request.parameters[0]);
            DateTime dateFrom = DateTime.Parse(request.parameters[1]);
            DateTime dateTo = DateTime.Parse(request.parameters[2]);
            int returnValue = service.commentsRepo.GetCommentCountBasedOnTimeSpan(postId, dateFrom, dateTo);
            Response<int> response = new Response<int>()
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

