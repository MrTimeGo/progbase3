using Progbase3ClassLib;
using Terminal.Gui;

namespace ConsoleApplication
{
    class Terminal
    {
        static Service service;
        public static void RunInterface(string databaseFilePath)
        {
            service = new Service(databaseFilePath);


            //Import.Run("./../../file.xml", service);
            Application.Init();

            MenuBar menu = new MenuBar(new MenuBarItem[] {
               new MenuBarItem ("_File", new MenuItem [] {
                   new MenuItem("_Export", "Export comments from post", OnExport),
                   new MenuItem("_Import", "Import comments from file", OnImport),
                   new MenuItem("_Exit", "Exit the program", OnExit)
               }),
           });

            Application.Top.Add(menu);

            Application.Run();
        }
        private static void OnExit()
        {
            Application.RequestStop();
        }
        private static void OnImport()
        {
            Window importWindow = new ImportWindow(service);
            Application.Run(importWindow);
        }
        private static void OnExport()
        {
            Window exportWindow = new ExportWindow(service);
            Application.Run(exportWindow);
        }
    }
}
