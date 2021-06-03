using System;


namespace Storage
{
    public class Post
    {
        public long id;
        public long authorId;
        public string title;
        public string text;
        public DateTime publishTime;


        public override string ToString()
        {
            string shortTitle = title.Length <= 30 ? title : title.Substring(0, 27) + "...";
            string shortText = text.Length <= 25 ? text : text.Substring(0, 22) + "...";
            return $"[{id}] {shortTitle.PadLeft(33)}  {shortText.PadLeft(28)} {publishTime.ToShortDateString()}";
        }

        public string GetStringRepresentation()
        {
            const string delimeter = "[~^";
            return $"{id}{delimeter}{authorId}{delimeter}" +
                $"{title}{delimeter}" +
                $"{text}{delimeter}" +
                $"{publishTime.ToString("o")}";
        }
        public static Post Parse(string representation)
        {
            const string delimeter = "[~^";
            string[] fields = representation.Split(delimeter);
            Post post = new Post()
            {
                id = long.Parse(fields[0]),
                authorId = long.Parse(fields[1]),
                title = fields[2],
                text = fields[3],
                publishTime = DateTime.Parse(fields[4])
            };
            return post;
        }
    }
}
