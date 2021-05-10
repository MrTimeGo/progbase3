using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace ConsoleApplication
{
    class Terminal
    {
        public static void RunInterface(string databaseFilePath)
        {
            Service service = new Service(databaseFilePath);


            //Import.Run("./../../file.xml", service);
            Application.Init();

            Window win = new UserCreationWindow(service);
            //Window win = new PostsListWindow(new List<Post>(service.postsRepo.GetAll()), service);

            Application.Run(win);
        }
    }
}
