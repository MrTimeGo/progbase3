using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Progbase3ClassLib
{
    public class RemotePostsRepository
    {
        Socket sender;
        public long Insert(Post post)
        {
            throw new NotImplementedException();
        }
        public Post GetById(long id)
        {
            throw new NotImplementedException();
        }
        public int Edit(Post editedPost)
        {
            throw new NotImplementedException();
        }
        public int DeleteById(long id)
        {
            throw new NotImplementedException();
        }
        public int GetTotalPages(string searchKeyword, long userId)
        {
            throw new NotImplementedException();
        }
        public List<Post> GetPage(int pageNumber, string searchKeyword, long authorId)
        {
            throw new NotImplementedException();
        }
    }
}
