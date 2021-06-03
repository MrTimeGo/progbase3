using System;
using System.Collections.Generic;
using Storage;
using System.Net.Sockets;
using System.Text;

namespace RPC
{
    public class RemotePostsRepository
    {
        Socket sender;
        public RemotePostsRepository(Socket sender)
        {
            this.sender = sender;
        }
        public long Insert(Post post)
        {
            List<string> parameters = new List<string>();
            parameters.Add(post.GetStringRepresentation());
            Request request = new Request()
            {
                methodName = "post.Insert",
                parameters = parameters
            };

            SendRequest(request);
            Response<long> response = GetResponse<long>();

            return response.returnValue;
        }
        public Post GetById(long id)
        {
            List<string> parameters = new List<string>();
            parameters.Add(id.ToString());
            Request request = new Request()
            {
                methodName = "post.GetById",
                parameters = parameters
            };

            SendRequest(request);
            Response<Post> response = GetResponse<Post>();

            return response.returnValue;
        }
        public int Edit(Post editedPost)
        {
            List<string> parameters = new List<string>();
            parameters.Add(editedPost.GetStringRepresentation());
            Request request = new Request()
            {
                methodName = "post.Edit",
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
                methodName = "post.DeleteById",
                parameters = parameters
            };

            SendRequest(request);
            Response<int> response = GetResponse<int>();

            return response.returnValue;
        }
        public int GetTotalPages(string searchKeyword, long userId)
        {
            List<string> parameters = new List<string>();
            parameters.Add(searchKeyword);
            parameters.Add(userId.ToString());
            Request request = new Request()
            {
                methodName = "post.GetTotalPages",
                parameters = parameters
            };

            SendRequest(request);
            Response<int> response = GetResponse<int>();

            return response.returnValue;
        }
        public List<Post> GetPage(int pageNumber, string searchKeyword, long authorId)
        {
            List<string> parameters = new List<string>();
            parameters.Add(pageNumber.ToString());
            parameters.Add(searchKeyword);
            parameters.Add(authorId.ToString());
            Request request = new Request()
            {
                methodName = "post.GetPage",
                parameters = parameters
            };

            SendRequest(request);
            Response<List<Post>> response = GetResponse<List<Post>>();

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
