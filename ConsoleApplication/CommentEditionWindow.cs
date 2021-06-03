using System;
using Terminal.Gui;
using Progbase3ClassLib;
using RPC;

namespace ConsoleApplication
{
    class CommentEditionWindow : Window
    {
        RemoteService service;
        Comment comment;

        TextView textView;

        public CommentEditionWindow(long commentId, RemoteService service)
        {
            this.service = service;
            this.comment = service.commentsRepo.GetById(commentId);

            this.Title = "Comment edition";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            Initialize();
        }
        private void Initialize()
        {
            Label labelPlainText = new Label("Text:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Height = 1
            };

            Window inputWindow = new Window()
            {
                X = Pos.Left(labelPlainText),
                Y = Pos.Bottom(labelPlainText) + 1,
                Width = Dim.Percent(80),
                Height = Dim.Fill(4)
            };
            textView = new TextView()
            {
                Height = Dim.Fill(),
                Width = Dim.Fill(),
                Text = comment.text
            };

            inputWindow.Add(textView);

            Button confirm = new Button("Edit")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(inputWindow) + 2,
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(confirm) + 3,
                Y = Pos.Top(confirm)
            };

            confirm.Clicked += OnConfirmClicked;
            cancel.Clicked += OnCancelClicked;

            this.Add(labelPlainText, inputWindow, confirm, cancel);
        }

        private void OnConfirmClicked()
        {
            if (textView.Text.ToString() == "")
            {
                MessageBox.ErrorQuery("Error", "Comment should not be empty", "Ok");
                return;
            }
            comment.text = textView.Text.ToString();
            service.commentsRepo.EditById(comment);

            MessageBox.Query("Info", "Comment was updated", "Ok");
            Application.RequestStop();
        }
        private void OnCancelClicked()
        {
            Application.RequestStop();
        }
    }
}
