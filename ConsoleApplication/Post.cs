﻿using System;
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

        public override string ToString()
        {
            string shortTitle = title.Length <= 30 ? title : title.Substring(0, 27) + "...";
            string shortText = text.Length <= 25 ? text : text.Substring(0, 22) + "...";
            return $"[{id}] {shortTitle.PadLeft(33)}  {shortText.PadLeft(28)} {publishTime.ToShortDateString()}";
        }
    }
}
