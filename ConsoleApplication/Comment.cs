using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Comment
    {
        public int id;
        public int authorId;
        public int postId;
        public string text;
        public DateTime publishTime;
        public bool isPinned;
    }
}
