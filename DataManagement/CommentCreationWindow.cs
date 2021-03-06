using System;
using Terminal.Gui;
using Storage;
using RPC;

namespace DataManagement
{
    class CommentCreationWindow : Window
    {
        TextView textView;

        RemoteService service;
        User logginedUser;
        long postId;
        public CommentCreationWindow(RemoteService service, long postId, User logginedUser)
        {
            this.Title = "New Comment";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            this.service = service;
            this.postId = postId;
            this.logginedUser = logginedUser;
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
            if (textView.Text.ToString() == "")
            {
                MessageBox.ErrorQuery("Error", "Comment should not be empty", "Ok");
                return;
            }
            Comment comment = new Comment()
            {
                authorId = logginedUser.id,
                postId = postId,
                text = textView.Text.ToString(),
                publishTime = DateTime.Now
            };
            service.commentsRepo.Insert(comment);

            MessageBox.Query("Info", "Comment was added", "Ok");

            Application.RequestStop();
        }
    }
}
