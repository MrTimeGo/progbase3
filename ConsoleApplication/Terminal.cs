using System;
using System.Collections.Generic;
using Terminal.Gui;
using Microsoft.Data.Sqlite;

namespace ConsoleApplication
{
    static class Terminal
    {
        static SqliteConnection connection;
        public static void RunInterface(string databaseFilePath)
        {
            connection = new SqliteConnection($"Data source = {databaseFilePath}");
            PostsRepository postRepo = new PostsRepository(connection);
            CommentsRepository commentRepo = new CommentsRepository(connection);

            Application.Init();

            Toplevel top = Application.Top;

            //Window win = new PostCreationWindow(repo);

            //Window win = new PostsListWindow(new List<Post>(postRepo.GetAll()));
            Window win = new CommentsListWindow(new List<Comment>(commentRepo.GetByPostId(10)));

            Application.Run(win);
        }

    }
}
