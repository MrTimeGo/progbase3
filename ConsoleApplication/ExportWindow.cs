using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class ExportWindow : Window
    {
        Service service;

        TextField postIdField;
        TextField directoryField;
        TextField fileNameField;

        public ExportWindow(Service service)
        {
            this.service = service;

            this.Title = "Export";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(30);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(40);

            Initialize();
        }
        private void Initialize()
        {
            Label postIdLabel = new Label("Post id:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
            };
            Label filePathLabel = new Label("Directory:")
            {
                X = Pos.Left(postIdLabel),
                Y = Pos.Bottom(postIdLabel) + 1,
            };
            Label fileNameLabel = new Label("File name:")
            {
                X = Pos.Left(postIdLabel),
                Y = Pos.Bottom(filePathLabel) + 1,
            };

            postIdField = new TextField()
            {
                X = Pos.Percent(60),
                Y = Pos.Top(postIdLabel),
                Width = Dim.Percent(30)
            };
            directoryField = new TextField()
            {
                X = Pos.Left(postIdField),
                Y = Pos.Top(filePathLabel),
                Width = Dim.Percent(30)
            };
            Button openDialog = new Button("...")
            {
                X = Pos.Right(directoryField),
                Y = Pos.Top(directoryField),
            };
            fileNameField = new TextField()
            {
                X = Pos.Left(directoryField),
                Y = Pos.Bottom(directoryField) + 1,
                Width = Dim.Percent(30) ,
            };
            Label xmlLabel = new Label(".xml")
            {
                X = Pos.Right(fileNameField),
                Y = Pos.Top(fileNameField)
            };

            Button export = new Button("Export")
            {
                X = Pos.Percent(50) - 11,
                Y = Pos.Bottom(xmlLabel) + 2
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(export) + 3,
                Y = Pos.Top(export)
            };

            export.Clicked += OnExportClicked;
            cancel.Clicked += OnCancelClicked;

            openDialog.Clicked += OnOpenDialogClicked;

            this.Add(postIdLabel, filePathLabel, fileNameLabel,
                postIdField, directoryField, openDialog, fileNameField, xmlLabel,
                export, cancel);
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnExportClicked()
        {
            if (!int.TryParse(postIdField.Text.ToString(), out int postId) && postId > 0)
            {
                MessageBox.ErrorQuery("Error", "Entered post id should be integer", "Ok");
                return;
            }
            string directoryPath = directoryField.Text.ToString();
            string fileName = fileNameField.Text.ToString();

            string filePath = string.Concat(directoryPath,"\\", fileName, ".xml");

            try
            {
                Export.Run(filePath, postId, service);
                MessageBox.Query("Info", $"All comments from post {postId} was exported", "Ok");
                Application.RequestStop();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "File path is wrong", "Ok");
            }
        }

        private void OnOpenDialogClicked()
        {
            OpenDialog dialog = new OpenDialog()
            {
                Title = "Choose directory",
                CanChooseDirectories = true,
                CanChooseFiles = false,
            };

            Application.Run(dialog);

            if (!dialog.Canceled)
            {
                directoryField.Text = dialog.FilePath;
            }
            Application.Refresh();
        }
    }
}
