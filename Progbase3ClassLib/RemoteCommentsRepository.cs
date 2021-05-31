using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Progbase3ClassLib
{
    public class RemoteCommentsRepository
    {
        Socket sender;
        public long Insert(Comment comment)
        {
            throw new NotImplementedException();
        }
        public Comment GetById(long id)
        {
            throw new NotImplementedException();
        }
        public int EditById(Comment editedComment)
        {
            throw new NotImplementedException();
        }
        public int DeleteById(long id)
        {
            throw new NotImplementedException();
        }
        public List<Comment> GetByPostId(long postId)
        {
            throw new NotImplementedException();
        }
        public int GetTotalPages(string searchKeyword, long id, bool isAuthor)
        {
            throw new NotImplementedException();
        }
        public List<Comment> GetPage(int pageNumber, string keyword, long id, bool isAuthor)
        {
            throw new NotImplementedException();
        }
        public Comment GetPinnedComment(long postId)
        {
            throw new NotImplementedException();
        }
        public int GetCommentCountBasedOnTimeSpan(long postId, DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }
    }
}
