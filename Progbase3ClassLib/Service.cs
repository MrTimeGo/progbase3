using System;
using Microsoft.Data.Sqlite;

namespace Storage
{
    public class Service
    {
        public UsersRepository usersRepo;
        public PostsRepository postsRepo;
        public CommentsRepository commentsRepo;

        public Service(string databaseFilePath)
        {
            SqliteConnection connection = new SqliteConnection($"Data source = {databaseFilePath}");
            usersRepo = new UsersRepository(connection);
            postsRepo = new PostsRepository(connection);
            commentsRepo = new CommentsRepository(connection);
        }
    }
}
