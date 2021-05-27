using System;


namespace Progbase3ClassLib
{
    public class Comment
    {
        public long id;
        public long authorId;
        public long postId;
        public string text;
        public DateTime publishTime;
        public bool isPinned;

        public override string ToString()
        {
            string shortText = text.Length <= 60 ? text : text.Substring(0, 57) + "...";
            return $"[{id}] {shortText} {publishTime.ToShortDateString()}";
        }
    }
}
