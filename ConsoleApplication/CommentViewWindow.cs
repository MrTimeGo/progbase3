using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class CommentViewWindow : Window
    {
        Comment comment;
        Service service;

        Label authorName;
        Label postPreview;
        Label text;
        public CommentViewWindow(long commentId, Service service)
        {
            this.comment = service.commentsRepo.GetById(commentId);
            this.service = service;

            this.Title = "Comment";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            Initialize();
        }
        private void Initialize()
        {
            authorName = new Label(service.usersRepo.GetById(comment.authorId).username)
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
            };

            Label creationTime = new Label(comment.publishTime.ToShortDateString())
            {
                X = Pos.Percent(70),
                Y = Pos.Top(authorName)
            };

            Window inputWindow = new Window()
            {
                X = Pos.Left(authorName),
                Y = Pos.Bottom(authorName) + 1,
                Width = Dim.Percent(80),
                Height = Dim.Fill(8)
            };

            text = new Label()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Text = comment.text
            };

            inputWindow.Add(text);

            Label postLabel = new Label("In post:")
            { 
                X = Pos.Left(inputWindow),
                Y = Pos.Bottom(inputWindow) + 1,
            };
            Post post = service.postsRepo.GetById(comment.postId);
            postPreview = new Label(service.postsRepo.GetById(comment.postId).ToString())
            {
                X = Pos.Left(postLabel),
                Y = Pos.Bottom(postLabel) + 1,
            };

            Button edit = new Button("Edit")
            {
                X = Pos.Percent(50) - 16,
                Y = Pos.Bottom(postPreview) + 2,
            };
            Button delete = new Button("Delete")
            {
                X = Pos.Right(edit) + 3,
                Y = Pos.Top(edit)
            };
            Button exit = new Button("Exit")
            {
                X = Pos.Right(delete) + 3,
                Y = Pos.Top(edit)
            };

            authorName.Clicked += OnAuthorClicked;
            postPreview.Clicked += OnPostPreviewClicked;

            edit.Clicked += OnEditClicked;
            delete.Clicked += OnDeleteClicked;
            exit.Clicked += OnExitClicked;

            this.Add(authorName, creationTime,
                inputWindow, postLabel, postPreview,
                edit, delete, exit);
        }

        private void OnPostPreviewClicked()
        {
            Window postInfo = new PostViewWindow(comment.postId, service);
            Application.Run(postInfo);

            UpdateWindow();
        }

        private void OnAuthorClicked()
        {
            Window userInfo = new UserViewWindow(comment.authorId, service);
            Application.Run(userInfo);

            UpdateWindow();
        }

        private void OnExitClicked()
        {
            Application.RequestStop();
        }

        private void OnDeleteClicked()
        {
            int result = MessageBox.Query("Info", "Are you sure, that you wand to delete comment", "Yes", "No");
            if (result == 0)
            {
                service.commentsRepo.DeleteById(comment.id);
                MessageBox.Query("Info", "Comment was deleted", "Ok");
                Application.RequestStop();
            }
        }

        private void OnEditClicked()
        {
            Window edition = new CommentEditionWindow(comment.id, service);
            Application.Run(edition);

            UpdateWindow();
        }
        private void UpdateWindow()
        {
            comment = service.commentsRepo.GetById(comment.id);

            text.Text = comment.text;
            authorName.Text = service.usersRepo.GetById(comment.authorId).username;
            postPreview.Text = service.postsRepo.GetById(comment.postId).ToString();
        }
    }
}
