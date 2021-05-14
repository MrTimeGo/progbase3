using System;
using System.Collections.Generic;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class PostViewWindow : Window
    {
        Post post;
        Service service;

        Label authorName;
        TextView text;
        Label titleLabel;
        public PostViewWindow(long postId, Service service)
        {
            this.post = service.postsRepo.GetById(postId);
            this.service = service;

            this.Title = "Post";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            Initialize();
        }
        private void Initialize()
        {
            titleLabel = new Label(post.title)
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Width = Dim.Percent(80),
                Height = 3
            };

            authorName = new Label($"By {service.usersRepo.GetById(post.authorId).username}")
            {
                X = Pos.Left(titleLabel),
                Y = Pos.Bottom(titleLabel) + 1
            };

            Label creationTime = new Label(post.publishTime.ToShortDateString())
            {
                X = Pos.Percent(70),
                Y = Pos.Top(authorName)
            };

            Window inputWindow = new Window()
            {
                X = Pos.Left(titleLabel),
                Y = Pos.Bottom(authorName),
                Width = Dim.Percent(80),
                Height = Dim.Fill(6)
            };

            text = new TextView()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
                Text = post.text
            };

            inputWindow.Add(text);

            Button allComments = new Button("View comments")
            {
                X = Pos.Percent(50) - 8,
                Y = Pos.Bottom(inputWindow) + 2
            };

            Button edit = new Button("Edit")
            {
                X = Pos.Percent(50) - 16,
                Y = Pos.Bottom(allComments) + 1,
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

            allComments.Clicked += OnAllCommentsClicked;
            authorName.Clicked += OnAuthorClicked;

            edit.Clicked += OnEditClicked;
            delete.Clicked += OnDeleteClicked;
            exit.Clicked += OnExitClicked;

            this.Add(titleLabel, authorName, creationTime,
                inputWindow,
                allComments,
                edit, delete, exit);
        }

        private void OnAuthorClicked()
        {
            Window infoWindow = new UserViewWindow(post.authorId, service);
            Application.Run(infoWindow);

            UpdateWindow();
        }

        private void OnExitClicked()
        {
            Application.RequestStop();
        }

        private void OnDeleteClicked()
        {
            int result = MessageBox.Query("Info", "Are you sure, that you wand to delete post", "Yes", "No");
            if (result == 0)
            {
                service.postsRepo.DeleteById(post.id);
                MessageBox.Query("Info", "Post was deleted", "Ok");
                Application.RequestStop();
            }
        }

        private void OnEditClicked()
        {
            Window editionWindow = new PostEditionWindow(post.id, service);
            Application.Run(editionWindow);

            UpdateWindow();
        }

        private void OnAllCommentsClicked()
        {
            Window listWindow = new CommentsListWindow(new List<Comment>(service.commentsRepo.GetByPostId(post.id)), service);
            Application.Run(listWindow);

            UpdateWindow();
        }
        private void UpdateWindow()
        {
            post = service.postsRepo.GetById(post.id);
            authorName.Text = service.usersRepo.GetById(post.authorId).username;
            titleLabel.Text = post.title;
            text.Text = post.text;
        }
    }
}
