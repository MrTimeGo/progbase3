using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace ConsoleApplication
{
    static class DataGenerator
    {
        public static void GenerateComments(int n, string databaseFileBase)
        {
            const string filePath = @".\..\data\generator\Comments.xml";
            List<Comment> list = new List<Comment>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            StreamReader fileStream = File.OpenText(filePath);
            XmlReader reader = XmlReader.Create(fileStream, settings);

            CommentsRepository repo = new CommentsRepository(databaseFileBase);

            int i = 0;
            while (reader.Read() && i < n)
            {
                Comment comment = new Comment();

                if (!int.TryParse(reader.GetAttribute("PostId"), out comment.postId))
                    continue;
                if (!int.TryParse(reader.GetAttribute("UserId"), out comment.authorId))
                    continue;
                comment.text = reader.GetAttribute("Text");
                if (!DateTime.TryParse(reader.GetAttribute("CreationDate"), out comment.publishTime))
                    continue;
                comment.isPinned = false;

                try
                {
                    repo.Insert(comment);
                    i++;
                }
                catch { }
            }

            reader.Dispose();
            fileStream.Dispose();
        }
        public static void GeneratePosts(int n, string databaseFileBase)
        {
            const string filePath = @".\..\data\generator\Posts.xml";
            const string titleFilePath = @".\..\data\generator\PostsTitle.xml";
            List<Post> list = new List<Post>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            StreamReader fileStream = File.OpenText(filePath);
            StreamReader fileStream2 = File.OpenText(titleFilePath);
            XmlReader reader = XmlReader.Create(fileStream, settings);
            XmlReader reader2 = XmlReader.Create(fileStream2, settings);

            PostsRepository repo = new PostsRepository(databaseFileBase);

            int i = 0;
            while (reader.Read() && reader2.Read() && i < n)
            { 
                Post post = new Post();
                if (!int.TryParse(reader.GetAttribute("UserId"), out post.authorId))
                    continue;
                post.title = reader2.GetAttribute("Title");
                post.text = reader.GetAttribute("Text");
                if (!DateTime.TryParse(reader.GetAttribute("CreationDate"), out post.publishTime))
                    continue;

                try
                {
                    repo.Insert(post);
                    i++;
                }
                catch { }
            }

            reader.Dispose();
            fileStream.Dispose();
        }
        public static void GenerateUsers(int n, string databaseFileBase)
        {
            const string filePath = @".\..\data\generator\Users.xml";
            List<User> list = new List<User>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            StreamReader fileStream = File.OpenText(filePath);
            XmlReader reader = XmlReader.Create(fileStream, settings);

            string[] genders = new string[] { "male", "female", "" };
            Random rnd = new Random();

            UsersRepository repo = new UsersRepository(databaseFileBase);

            int i = 0;
            while (reader.Read() && i < n)
            {
                User user = new User
                {
                    username = reader.GetAttribute("DisplayName"),
                    password = CreatePassword(12),
                    gender = genders[rnd.Next(genders.Length)],
                    isModerator = false
                };

                try
                {
                    repo.Insert(user);
                    i++;
                }
                catch { }

            }

            reader.Dispose();
            fileStream.Dispose();
        }
        static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
