using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Storage;

namespace RPC
{
    public class RemoteCommentsRepository
    {
        Socket sender;
        public RemoteCommentsRepository(Socket sender)
        {
            this.sender = sender;
        }
        public long Insert(Comment comment)
        {
            List<string> parameters = new List<string>();
            parameters.Add(comment.GetStringRepresentation());
            Request request = new Request()
            {
                methodName = "comment.Insert",
                parameters = parameters
            };

            SendRequest(request);
            Response<long> response = GetResponse<long>();

            return response.returnValue;
        }
        public Comment GetById(long id)
        {
            List<string> parameters = new List<string>();
            parameters.Add(id.ToString());
            Request request = new Request()
            {
                methodName = "comment.GetById",
                parameters = parameters
            };

            SendRequest(request);
            Response<Comment> response = GetResponse<Comment>();

            return response.returnValue;
        }
        public int EditById(Comment editedComment)
        {
            List<string> parameters = new List<string>();
            parameters.Add(editedComment.GetStringRepresentation());
            Request request = new Request()
            {
                methodName = "comment.EditById",
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
                methodName = "comment.DeleteById",
                parameters = parameters
            };

            SendRequest(request);
            Response<int> response = GetResponse<int>();

            return response.returnValue;
        }
        public List<Comment> GetByPostId(long postId)
        {
            List<string> parameters = new List<string>();
            parameters.Add(postId.ToString());
            Request request = new Request()
            {
                methodName = "comment.GetByPostId",
                parameters = parameters
            };

            SendRequest(request);
            Response<List<Comment>> response = GetResponse<List<Comment>>();

            return response.returnValue;
        }
        public int GetTotalPages(string searchKeyword, long id, bool isAuthor)
        {
            List<string> parameters = new List<string>();
            parameters.Add(searchKeyword);
            parameters.Add(id.ToString());
            parameters.Add(isAuthor.ToString());
            Request request = new Request()
            {
                methodName = "comment.GetTotalPages",
                parameters = parameters
            };

            SendRequest(request);
            Response<int> response = GetResponse<int>();

            return response.returnValue;
        }
        public List<Comment> GetPage(int pageNumber, string keyword, long id, bool isAuthor)
        {
            List<string> parameters = new List<string>();
            parameters.Add(pageNumber.ToString());
            parameters.Add(keyword);
            parameters.Add(id.ToString());
            parameters.Add(isAuthor.ToString());
            Request request = new Request()
            {
                methodName = "comment.GetPage",
                parameters = parameters
            };

            SendRequest(request);
            Response<List<Comment>> response = GetResponse<List<Comment>>();

            return response.returnValue;
        }
        public Comment GetPinnedComment(long postId)
        {
            List<string> parameters = new List<string>();
            parameters.Add(postId.ToString());
            Request request = new Request()
            {
                methodName = "comment.GetPinnedComment",
                parameters = parameters
            };

            SendRequest(request);
            Response<Comment> response = GetResponse<Comment>();

            return response.returnValue;
        }
        public int GetCommentCountBasedOnTimeSpan(long postId, DateTime dateFrom, DateTime dateTo)
        {
            List<string> parameters = new List<string>();
            parameters.Add(postId.ToString());
            parameters.Add(dateFrom.ToString("o"));
            parameters.Add(dateTo.ToString("o"));
            Request request = new Request()
            {
                methodName = "comment.GetCommentCountBasedOnTimeSpan",
                parameters = parameters
            };

            SendRequest(request);
            Response<int> response = GetResponse<int>();

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
