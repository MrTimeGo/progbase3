using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class ImportWindow : Window
    {
        Service service;

        TextField filePathField;
        public ImportWindow(Service service)
        {
            this.service = service;

            this.Title = "Import";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(30);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(40);

            Initialize();
        }
        private void Initialize()
        {
            Label filePathLabel = new Label("File path:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(50) - 2,
            };

            filePathField = new TextField()
            {
                X = Pos.Percent(60),
                Y = Pos.Top(filePathLabel),
                Width = Dim.Percent(30)
            };
            Button openDialog = new Button("...")
            {
                X = Pos.Right(filePathField),
                Y = Pos.Top(filePathField),
            };
            

            Button import = new Button("Import")
            {
                X = Pos.Percent(50) - 11,
                Y = Pos.Bottom(openDialog) + 2
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(import) + 3,
                Y = Pos.Top(import)
            };

            import.Clicked += OnImportClicked;
            cancel.Clicked += OnCancelClicked;

            openDialog.Clicked += OnOpenDialogClicked;

            this.Add(filePathLabel, filePathField, openDialog,
                import, cancel);
        }

        private void OnImportClicked()
        {
            string filePath = filePathField.Text.ToString();
            
            try
            {
                Import.Run(filePath, service);
                MessageBox.Query("Info", "Comments were imported", "Ok");
                Application.RequestStop();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", $"Cannot import comments:\n{ex.Message}", "Ok");
            }
        }
        private void OnCancelClicked()
        {
            Application.RequestStop();
        }
        private void OnOpenDialogClicked()
        {
            OpenDialog dialog = new OpenDialog()
            {
                Title = "Choose directory",
                CanChooseFiles = true,
            };

            Application.Run(dialog);

            if (!dialog.Canceled)
            {
                filePathField.Text = dialog.FilePath;
            }
        }
    }
}
