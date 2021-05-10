using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace ConsoleApplication
{
    class UserViewWindow : Window
    {
        User user;
        Service service;

        TextField usernameField;
        TextField genderField;

        public UserViewWindow(long userId, Service service)
        {
            this.user = service.usersRepo.GetById(userId);
            this.service = service;

            this.Title = "User Info";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            Initialize();
        }
        private void Initialize()
        {
            Label labelUsername = new Label("Username:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(50) - 6,
                Height = 1
            };

            Label labelGender = new Label("Gender:")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelUsername) + 1,
                Height = 1
            };

            Label labelRegisteredAt = new Label("Registration date: ")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelGender) + 1
            };

            usernameField = new TextField(user.username)
            {
                X = Pos.Percent(60),
                Y = Pos.Top(labelUsername),
                Height = 1,
                ReadOnly = true,
                AutoSize = true
            };

            genderField = new TextField(user.gender)
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelGender),
                ReadOnly = true,
                AutoSize = true
            };

            TextField registeredAtField = new TextField(user.createdAt.ToShortDateString())
            {
                X = Pos.Left(genderField),
                Y = Pos.Top(labelRegisteredAt),
                ReadOnly = true
            };

            Button allPosts = new Button("View all posts")
            {
                X = Pos.Percent(50) - 21,
                Y = Pos.Bottom(labelRegisteredAt) + 3
            };
            Button allComments = new Button("View all comments")
            {
                X = Pos.Right(allPosts) + 3,
                Y = Pos.Top(allPosts)
            };

            Button edit = new Button("Edit")
            {
                X = Pos.Percent(50) - 16,
                Y = Pos.Bottom(allPosts) + 1
            };
            Button delete = new Button("Delete")
            {
                X = Pos.Right(edit) + 3,
                Y = Pos.Top(edit)
            };
            Button exit = new Button("Exit")
            {
                X = Pos.Right(delete) + 3,
                Y = Pos.Top(delete)
            };

            allPosts.Clicked += OnAllPostsClicked;
            allComments.Clicked += OnAllCommentsClicked;

            edit.Clicked += OnEditClicked;
            delete.Clicked += OnDeleteClicked;
            exit.Clicked += OnExitClicked;

            this.Add(labelUsername, labelGender, labelRegisteredAt,
                usernameField, genderField, registeredAtField,
                allPosts, allComments,
                edit, delete, exit);
        }

        private void OnAllCommentsClicked()
        {
            Window allComments = new CommentsListWindow(new List<Comment>(service.commentsRepo.GetByUserId(user.id)), service);
            Application.Run(allComments);

            UpdateWindow();
        }

        private void OnAllPostsClicked()
        {
            Window allPosts = new PostsListWindow(new List<Post>(service.postsRepo.GetByUserId(user.id)), service);
            Application.Run(allPosts);

            UpdateWindow();
        }

        private void OnExitClicked()
        {
            Application.RequestStop();
        }

        private void OnDeleteClicked()
        {
            int result = MessageBox.Query("Info", "Are you sure, that you wand to delete user", "Yes", "No");
            if (result == 0)
            {
                service.usersRepo.DeleteById(user.id);
                MessageBox.Query("Info", "User was deleted", "Ok");
                Application.RequestStop();
            }
        }

        private void OnEditClicked()
        {
            Window editionWindow = new UserEditionWindow(user.id, service);
            Application.Run(editionWindow);

            UpdateWindow();
        }
        private void UpdateWindow()
        {
            user = service.usersRepo.GetById(user.id);
            usernameField.Text = user.username;
            genderField.Text = user.gender;
        }
    }
}
