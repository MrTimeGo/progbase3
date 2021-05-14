using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class CommentCreationWindow : Window
    {
        TextView textView;

        CommentsRepository repository;
        public CommentCreationWindow(Service service)
        {
            this.Title = "New Comment";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            this.repository = service.commentsRepo;
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
                Width = Dim.Fill()
            };

            inputWindow.Add(textView);

            Button createNewComment = new Button("Publish")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(inputWindow) + 2,
            };
            Button cancelComment = new Button("Cancel")
            {
                X = Pos.Right(createNewComment) + 3,
                Y = Pos.Top(createNewComment)
            };

            createNewComment.Clicked += OnCreateNewCommentClicked;
            cancelComment.Clicked += OnCancelClicked;

            this.Add(labelPlainText, inputWindow, createNewComment, cancelComment);
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnCreateNewCommentClicked()
        {
            Comment comment = new Comment()
            {
                text = textView.Text.ToString(),
                publishTime = DateTime.Now
            };
            repository.Insert(comment);

            MessageBox.Query("Info", "Comment was added", "Ok");

            Application.RequestStop();
        }
    }
}
