using Storage;
using Terminal.Gui;
using System;
using RPC;

namespace DataManagement
{
    class Terminal
    {
        RemoteService service;
        public void RunInterface()
        {  
            service = new RemoteService();

            if (!service.TryConnect())
            {
                Console.WriteLine("Can't connect to the server");
                return;
            }

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
               }),
               new MenuBarItem("_Help", new MenuItem[]
               {
                   new MenuItem("_About", "Get information about program", OnAbout)
               })
            });
            MainWindow win = new MainWindow(service, null);

            Application.Top.Add(menu, win);

            Application.Run();
        }
        private void OnExit()
        {
            Application.Top.RemoveAll();
            Application.RequestStop();
        }
        private void OnImport()
        {
            Window importWindow = new ImportWindow(service);
            Application.Run(importWindow);
        }
        private void OnExport()
        {
            Window exportWindow = new ExportWindow(service);
            Application.Run(exportWindow);
        }
        private void OnGenerateReport()
        {
            ReportDialog reportDialog = new ReportDialog(service);
            Application.Run(reportDialog);
        }
        private void OnAbout()
        {
            //Dialog aboutDialog = new Dialog()
            //{
            //    X = Pos.Center(),
            //    Y = Pos.Center(),
            //    Width = Dim.Percent(50),
            //    Height = Dim.Percent(50)
            //};
            //Label info
            MessageBox.Query("About", "Social network \"BookFace\" by Artem Petselia, KP-01\nHere you can write, comment, discuss.", "Ok");
        }
    }
}
