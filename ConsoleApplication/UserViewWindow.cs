using System;
using System.Collections.Generic;
using Terminal.Gui;
using RPC;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class UserViewWindow : Window
    {
        User user;
        RemoteService service;
        User loggedUser;

        TextField usernameField;
        TextField genderField;
        DateField birthDateField;

        public UserViewWindow(long userId, RemoteService service, User loggedUser)
        {
            this.user = service.usersRepo.GetById(userId);
            this.service = service;
            this.loggedUser = loggedUser;

            this.Title = "User Info";
            Y = Pos.Percent(0) + 1;
            this.Width = Dim.Fill();
            this.Height = Dim.Fill();

            Initialize();
        }
        private void Initialize()
        {
            Label labelUsername = new Label("Username:")
            {
                X = Pos.Percent(15),
                Y = Pos.Percent(50) - 6,
                Height = 1
            };
            Label labelBirthDate = new Label("Date of birth:")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelUsername) + 1,
                Height = 1
            };

            Label labelGender = new Label("Gender:")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelBirthDate) + 1,
                Height = 1
            };

            

            Label labelRegisteredAt = new Label("Registration date: ")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelGender) + 1
            };

            usernameField = new TextField(user.username)
            {
                X = Pos.Percent(70),
                Y = Pos.Top(labelUsername),
                Height = 1,
                Width = Dim.Percent(20),
                ReadOnly = true,
            };

            genderField = new TextField(ChooseGender(user.gender))
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelGender),
                Width = Dim.Percent(20),
                ReadOnly = true,
            };

            birthDateField = new DateField(user.birthDate)
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelBirthDate),
                ReadOnly = true,
            };

            TextField registeredAtField = new TextField(user.createdAt.ToShortDateString())
            {
                X = Pos.Left(usernameField),
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
            Button exit = new Button("Exit")
            {
                X = Pos.Right(edit) + 3,
                Y = Pos.Top(edit)
            };
            Button delete = new Button("Delete")
            {
                X = Pos.Right(exit) + 3,
                Y = Pos.Top(exit)
            };
            Button toMainWindow = new Button("To main window")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(delete) + 1
            };

            if (user.id != loggedUser.id)
            {
                edit.Visible = false;
                delete.Visible = false;
            }
            

            allPosts.Clicked += OnAllPostsClicked;
            allComments.Clicked += OnAllCommentsClicked;

            edit.Clicked += OnEditClicked;
            delete.Clicked += OnDeleteClicked;
            exit.Clicked += OnExitClicked;

            toMainWindow.Clicked += OnMainWinowClicked;

            this.Add(labelUsername, labelBirthDate, labelGender, labelRegisteredAt,
                usernameField, genderField, birthDateField, registeredAtField,
                allPosts, allComments,
                edit, exit, delete, toMainWindow);
        }

        private void OnMainWinowClicked()
        {
            MainWindow win = new MainWindow(service, loggedUser);
            MenuBar menu = Application.Top.MenuBar;
            Application.Top.RemoveAll();
            Application.Top.Add(menu, win);
            Application.RequestStop();
            Application.Run();
        }

        private string ChooseGender(int i)
        {
            string[] validGenders = new string[] { "other", "male", "female" };
            return validGenders[i];
        }
        private void OnAllCommentsClicked()
        {
            Window allComments = new CommentsListWindow(true, user.id, service, loggedUser);
            Application.Top.Add(allComments);
            Application.RequestStop();
            Application.Run();

            UpdateWindow();
        }

        private void OnAllPostsClicked()
        {
            Window allPosts = new PostsListWindow(user.id, service, loggedUser);
            Application.Top.Add(allPosts);
            Application.RequestStop();
            Application.Run();

            UpdateWindow();
        }

        private void OnExitClicked()
        {
            Application.Top.Remove(this);
        }

        private void OnDeleteClicked()
        {
            int result = MessageBox.Query("Info", "Are you sure, that you wand to delete user", "Yes", "No");
            if (result == 0)
            {
                service.usersRepo.DeleteById(user.id);
                MessageBox.Query("Info", "User was deleted", "Ok");
                Application.Top.Remove(this);
                if (user.id == loggedUser.id)
                {
                    LoginDialog loginDialog = new LoginDialog(service);
                    Application.Run(loginDialog);
                    loggedUser.id = loginDialog.loggedInUser.id;
                    loggedUser.username = loginDialog.loggedInUser.username;
                    loggedUser.password = loginDialog.loggedInUser.password;
                    loggedUser.birthDate = loginDialog.loggedInUser.birthDate;
                    loggedUser.isModerator = loginDialog.loggedInUser.isModerator;
                    loggedUser.gender = loginDialog.loggedInUser.gender;
                    loggedUser.createdAt = loginDialog.loggedInUser.createdAt;
                }
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
            genderField.Text = ChooseGender(user.gender);
            birthDateField.Date = user.birthDate;
            if (user.id == loggedUser.id)
            {
                loggedUser.id = user.id;
                loggedUser.username = user.username;
                loggedUser.password = user.password;
                loggedUser.birthDate = user.birthDate;
                loggedUser.isModerator = user.isModerator;
                loggedUser.gender = user.gender;
                loggedUser.createdAt = user.createdAt;
            }
        }
    }
}
