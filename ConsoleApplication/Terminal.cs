using Progbase3ClassLib;
using Terminal.Gui;

namespace ConsoleApplication
{
    class Terminal
    {
        static RemoteService service;
        public static void RunInterface()
        {
            service = new RemoteService();


            //Import.Run("./../../file.xml", service);
            Application.Init();

            MenuBar menu = new MenuBar(new MenuBarItem[] 
            {
               new MenuBarItem ("_File", new MenuItem [] 
               {
                   new MenuItem("_Export", "Export comments from post", OnExport),
                   new MenuItem("_Import", "Import comments from file", OnImport),
                   new MenuItem("_Exit", "Exit the program", OnExit)
               }),
               new MenuBarItem ("_Report", new MenuItem[]
               {
                   new MenuItem("_New report", "Generate new report", OnGenerateReport)
               })
            });
            MainWindow win = new MainWindow(service, null);

            Application.Top.Add(menu, win);

            Application.Run();
        }
        private static void OnExit()
        {
            Application.Top.RemoveAll();
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
        private static void OnGenerateReport()
        {
            ReportDialog reportDialog = new ReportDialog(service);
            Application.Run(reportDialog);
        }
    }
}
