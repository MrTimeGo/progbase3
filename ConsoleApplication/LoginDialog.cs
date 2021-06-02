using System;
using Progbase3ClassLib;
using Terminal.Gui;

namespace ConsoleApplication
{
    class LoginDialog : Dialog
    {
        public User loggedInUser;
        RemoteService service;

        TextField username;
        TextField password;
        public LoginDialog(RemoteService service)
        {
            this.Title = "Login";
            this.X = Pos.Center();
            this.Y = Pos.Center();
            this.Width = Dim.Percent(30);
            this.Height = Dim.Percent(60);
            this.service = service;

            Initialize();
        }
        private void Initialize()
        {
            Label labelUsername = new Label("Username:")
            {
                X = Pos.Percent(10),
                Y = Pos.Center() - 7
            };
            username = new TextField()
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelUsername) + 1,
                Width = Dim.Percent(80)
            };
            Label labelPassword = new Label("Password:")
            {
                X = Pos.Left(username),
                Y = Pos.Bottom(username) + 1
            };
            password = new TextField()
            {
                X = Pos.Left(labelPassword),
                Y = Pos.Bottom(labelPassword) + 1,
                Width = Dim.Percent(80),
                Secret = true
            };

            Button login = new Button("Log in")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(password) + 2
            };
            Label labelCreationAccount = new Label("Don't have account?")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(login) + 2,
            };
            Button signUp = new Button("Sign up")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(labelCreationAccount) + 1,
            };

            login.Clicked += OnLoginClicked;
            signUp.Clicked += OnSignUpClicked;

            this.Add(labelUsername, username, labelPassword, password, login, labelCreationAccount, signUp);
        }

        private void OnSignUpClicked()
        {
            UserCreationWindow registration = new UserCreationWindow(service);
            Application.Run(registration);
        }

        private void OnLoginClicked()
        {
            string username = this.username.Text.ToString();
            string password = this.password.Text.ToString();
            Authentication auth = new Authentication(service);
            try
            {
                loggedInUser = auth.Login(username, password);
                Application.RequestStop();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "Ok");
            }
        }
    }
}
