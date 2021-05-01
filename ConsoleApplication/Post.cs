using System;
using System.Collections.Generic;


namespace ConsoleApplication
{
    class Post
    {
        public long id;
        public long authorId;
        public string title;
        public string text;
        public DateTime publishTime;
        public List<Comment> comments;
    }
}
