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
        public string GetStringRepresentation()
        {
            const string delimeter = "[~^";
            return $"{id}{delimeter}" +
                $"{authorId}{delimeter}" +
                $"{postId}{delimeter}" +
                $"{text}{delimeter}" +
                $"{publishTime.ToString("o")}{delimeter}" +
                $"{isPinned}";
        }
        public static Comment Parse(string representation)
        {
            const string delimeter = "[~^";
            string[] fields = representation.Split(delimeter);
            Comment comment = new Comment()
            {
                id = long.Parse(fields[0]),
                authorId = long.Parse(fields[1]),
                postId = long.Parse(fields[2]),
                text = fields[3],
                publishTime = DateTime.Parse(fields[4]),
                isPinned = bool.Parse(fields[5])
            };
            return comment;
        }
    }
}
