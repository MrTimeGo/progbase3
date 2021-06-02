using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class ReportDialog : Dialog
    {
        RemoteService service;

        TextField postIdField;
        TextField pathTextField;
        DateField dateFromField;
        TimeField timeFromField;
        DateField dateToField;
        TimeField timeToField;
        public ReportDialog(RemoteService service)
        {
            this.service = service;

            Title = "Report generation";
            X = Pos.Center();
            Y = Pos.Center();
            Width = Dim.Percent(40);
            Height = Dim.Percent(60);
            Initialize();
        }
        void Initialize()
        {
            Label saveToPath = new Label("Save to:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(50) - 6
            };
            pathTextField = new TextField()
            {
                X = Pos.Percent(10),
                Y = Pos.Bottom(saveToPath) + 1,
                Width = Dim.Percent(80) - 5
            };
            Button openDialog = new Button("...")
            {
                X = Pos.Right(pathTextField),
                Y = Pos.Top(pathTextField),
            };
            Label postidLabel = new Label("Post id:")
            {
                X = Pos.Left(pathTextField),
                Y = Pos.Bottom(pathTextField) + 1
            };
            postIdField = new TextField()
            {
                X = Pos.Right(postidLabel) + 3,
                Y = Pos.Top(postidLabel),
                Width = 4
            };
            Label timespanLabel = new Label("Time span:")
            {
                X = Pos.Left(postidLabel),
                Y = Pos.Bottom(postidLabel) + 1
            };
            Label delimeterLabel = new Label("-")
            { 
                X = Pos.Center(),
                Y = Pos.Bottom(timespanLabel) + 1
            };
            timeFromField = new TimeField()
            {
                X = Pos.Left(delimeterLabel) - 8,
                Y = Pos.Top(delimeterLabel)
            };
            dateFromField = new DateField()
            {
                X = Pos.Left(timeFromField) - 11,
                Y = Pos.Top(timeFromField)
            };
            dateToField = new DateField()
            {
                X = Pos.Right(delimeterLabel) + 1,
                Y = Pos.Top(delimeterLabel)
            };
            timeToField = new TimeField()
            { 
                X = Pos.Right(dateToField) + 1,
                Y = Pos.Top(dateToField)
            };
            Button generate = new Button("Generate")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(delimeterLabel) + 2
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(generate) + 3,
                Y = Pos.Top(generate)
            };

            openDialog.Clicked += OnOpenDialogClicked;
            generate.Clicked += OnGenerateClicked;
            cancel.Clicked += OnCancelClicked;

            this.Add(saveToPath, pathTextField, openDialog, 
                postidLabel, postIdField, timespanLabel,
                delimeterLabel, timeFromField, dateFromField, dateToField, timeToField,
                generate, cancel);
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnGenerateClicked()
        {
            if (!long.TryParse(postIdField.Text.ToString(), out long postId))
            {
                MessageBox.ErrorQuery("Error", "Wrong post id", "Ok");
                return;
            }
            if (pathTextField.Text.ToString() == "")
            {
                MessageBox.ErrorQuery("Error", "Directory path", "Ok");
                return;
            }
            DateTime dateFrom = dateFromField.Date + timeFromField.Time;
            DateTime dateTo = dateToField.Date + timeToField.Time;
            if (dateFrom > dateTo)
            {
                MessageBox.ErrorQuery("Error", "Wrong date path", "Ok");
                return;
            }

            try
            {
                ReportGenerator.GenerateNewReport(postId, dateFrom, dateTo, service, pathTextField.Text.ToString());
                Application.RequestStop();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "Some file is busy", "Ok");
            }
        }

        private void OnOpenDialogClicked()
        {
            OpenDialog openDialog = new OpenDialog("Choose file", "Choose");
            openDialog.CanCreateDirectories = true;
            openDialog.CanChooseFiles = false;
            openDialog.CanChooseDirectories = true;
            Application.Run(openDialog);
            if (!openDialog.Canceled)
            {
                pathTextField.Text = openDialog.DirectoryPath;
            }
        }
    }
}
