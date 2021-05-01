using System;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Data.Sqlite;

namespace ConsoleApplication
{
    static class DataGenerator
    {
        public static void GenerateComments(SqliteConnection connection, int number, DateTime dateFrom, DateTime dateTo)
        {
            CommentsRepository commentsRepository = new CommentsRepository(connection);
            PostsRepository postsRepository = new PostsRepository(connection);
            UsersRepository usersRepository = new UsersRepository(connection);

            long[] userIds = GetUserIds(number, usersRepository);
            long[] postIds = GetPostIds(number, postsRepository);
            string[] commentTexts = GetCommentText(number);
            DateTime[] dateTimes = GetDateTimes(number, dateFrom, dateTo);

            for (int i = 0; i < number; i++)
            {
                Comment comment = new Comment
                {
                    authorId = userIds[i],
                    postId = postIds[i],
                    text = commentTexts[i],
                    publishTime = dateTimes[i],
                    isPinned = false
                };
                try
                {
                    commentsRepository.Insert(comment);
                }
                catch
                {
                    connection.Close();
                }
            }
        }
        private static string[] GetCommentText(int n)
        {
            const string commentTextDatasetPath = @".\..\data\generator\Comments.xml";
            string[] commentTexts = new string[n];

            XmlReader reader = GetReader(commentTextDatasetPath);

            int i = 0;
            while (reader.Read() && i < n)
            {
                commentTexts[i] = reader.GetAttribute("Text");
                i++;
            }
            reader.Close();
            return commentTexts;
        }
        private static long[] GetPostIds(int n, PostsRepository repo)
        {
            Post[] posts = repo.GetAll();
            if (posts.Length == 0)
            {
                throw new Exception($"Cannot generate posts ids, because there is no posts in database");
            }
            Random rnd = new Random();

            long[] postIds = new long[n];

            for (int i = 0; i < postIds.Length; i++)
            {
                postIds[i] = posts[rnd.Next(0, posts.Length)].id;
            }
            return postIds;
        }
        public static void GeneratePosts(SqliteConnection connection, int number, DateTime dateFrom, DateTime dateTo)
        {
            PostsRepository postsRepository = new PostsRepository(connection);
            UsersRepository usersRepository = new UsersRepository(connection);

            long[] userIds = GetUserIds(number, usersRepository);
            string[] postTitles = GetPostTitles(number, postsRepository);
            string[] postTexts = GetPostTexts(number);
            DateTime[] dateTimes = GetDateTimes(number, dateFrom, dateTo);

            for (int i = 0; i < number; i++)
            {
                Post post = new Post()
                { 
                    authorId = userIds[i],
                    title = postTitles[i],
                    text = postTexts[i],
                    publishTime = dateTimes[i]
                };
                try
                {
                    postsRepository.Insert(post);
                }
                catch
                {
                    connection.Close();
                }
            }
        }
        private static string[] GetPostTexts(int n)
        {
            const string postTextDatasetPath = @".\..\data\generator\Posts.xml";
            string[] postTexts = new string[n];

            XmlReader reader = GetReader(postTextDatasetPath);

            int i = 0;
            while (reader.Read() && i < n)
            {
                postTexts[i] = reader.GetAttribute("Text");
                i++;
            }
            reader.Close();
            return postTexts;
        }
        private static string[] GetPostTitles(int n, PostsRepository repo)
        {
            const string postTitlesDatasetPath = @".\..\data\generator\PostsTitle.xml";
            string[] postTitles = new string[n];

            XmlReader reader = GetReader(postTitlesDatasetPath);

            int i = 0;
            while (reader.Read() && i < n)
            {
                string postTitle = reader.GetAttribute("Title");
                if (postTitle == null)
                {
                    continue;
                }
                if (!repo.PostExists(postTitle))
                {
                    postTitles[i] = postTitle;
                    i++;
                }
            }
            reader.Close();
            return postTitles;
        }
        private static long[] GetUserIds(int n, UsersRepository repo)
        {
            User[] users = repo.GetAll();
            if (users.Length == 0)
            {
                throw new Exception($"Cannot generate users ids, because there is no users in database");
            }
            Random rnd = new Random();

            long[] userIds = new long[n];
            
            for (int i = 0; i < userIds.Length; i++)
            {
                userIds[i] = users[rnd.Next(0, users.Length)].id;
            }
            return userIds;
        }
        public static void GenerateUsers(SqliteConnection connection, int number, DateTime dateFrom, DateTime dateTo)
        {
            UsersRepository repository = new UsersRepository(connection);

            string[] usernames = GetUsernames(number, repository);
            string[] passwords = GetPasswords(number);
            string[] genders = GetGenders(number);
            DateTime[] dateTimes = GetDateTimes(number, dateFrom, dateTo);

            for (int i = 0; i < number; i++)
            {
                User user = new User()
                { 
                    username = usernames[i],
                    password = passwords[i],
                    isModerator = false,
                    gender = genders[i],
                    createdAt = dateTimes[i]
                };
                try
                {
                    repository.Insert(user);
                }
                catch
                {
                    connection.Close();
                }
            }
        }
        private static DateTime[] GetDateTimes(int n, DateTime dateFrom, DateTime dateTo)
        {
            Random rnd = new Random();
            DateTime[] dateTimes = new DateTime[n];

            int range = (dateTo - dateFrom).Days;
            for (int i = 0; i < dateTimes.Length; i++)
            {
                dateTimes[i] = dateFrom.AddDays(rnd.Next(range));
            }
            return dateTimes;
        }
        private static string[] GetGenders(int n)
        {
            string[] validGenders = new string[] { "male", "female", "other"};
            Random rnd = new Random();

            string[] genders = new string[n];
            for (int i = 0; i < genders.Length; i++)
            {
                genders[i] = validGenders[rnd.Next(0, 3)];
            }
            return genders;
        }
        private static string[] GetPasswords(int n)
        {
            Random rnd = new Random();
            string[] passwords = new string[n];
            for (int i = 0; i < passwords.Length; i++)
            {
                passwords[i] = CreatePassword(rnd.Next(10, 16));
            }
            return passwords;
        }
        private static string[] GetUsernames(int n, UsersRepository repo)
        {
            const string usernamesDatasetPath = @".\..\data\generator\Users.xml";
            string[] usernames = new string[n];

            XmlReader reader = GetReader(usernamesDatasetPath);

            int i = 0;
            while (reader.Read() && i < n)
            {
                string username = reader.GetAttribute("DisplayName");
                if (username == null)
                {
                    continue;
                }
                if (!repo.UserExists(username))
                {
                    usernames[i] = username;
                    i++;
                }
            }
            reader.Close();
            return usernames;
        }
        private static XmlReader GetReader(string filePath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            StreamReader fileStream = File.OpenText(filePath);
            XmlReader reader = XmlReader.Create(fileStream, settings);
            return reader;
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
