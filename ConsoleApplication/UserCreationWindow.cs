using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class UserCreationWindow : Window
    {
        TextField usernameField;
        TextField passwordField;
        TextField confirmedPasswordField;
        RadioGroup genderRadioGroup;

        UsersRepository repository;

        public UserCreationWindow(Service service)
        {
            this.Title = "Registration";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            repository = service.usersRepo;

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
            Label labelPassword = new Label("Password:")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelUsername) + 1,
                Height = 1
            };
            Label labelConfirmPassword = new Label("Confirm password:")
            {
                X = Pos.Left(labelPassword),
                Y = Pos.Bottom(labelPassword) + 1,
                Height = 1
            };
            Label labelGender = new Label("Gender:")
            {
                X = Pos.Left(labelConfirmPassword),
                Y = Pos.Bottom(labelConfirmPassword) + 1,
                Height = 1
            };

            usernameField = new TextField()
            {
                X = Pos.Percent(60),
                Y = Pos.Top(labelUsername),
                Height = 1,
                Width = Dim.Percent(30)
            };
            passwordField = new TextField()
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelPassword),
                Height = 1,
                Width = Dim.Width(usernameField),
                Secret = true
            };
            confirmedPasswordField = new TextField()
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelConfirmPassword),
                Height = 1,
                Width = Dim.Width(usernameField),
                Secret = true
            };
            genderRadioGroup = new RadioGroup(new NStack.ustring[] { "Male", "Female", "Other" })
            {
                X = Pos.Left(confirmedPasswordField),
                Y = Pos.Top(labelGender),
            };


            Button createNewUser = new Button("Register")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(genderRadioGroup) + 2,
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(createNewUser) + 3,
                Y = Pos.Top(createNewUser)
            };

            createNewUser.Clicked += OnCreateNewUserClicked;
            cancel.Clicked += OnCancelClicked;

            this.Add(labelUsername, labelPassword, labelConfirmPassword, labelGender,
                usernameField, passwordField, confirmedPasswordField, genderRadioGroup,
                createNewUser, cancel);
        }
        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnCreateNewUserClicked()
        {
            if (passwordField.Text == confirmedPasswordField.Text)
            {
                User user = new User()
                {
                    username = usernameField.Text.ToString(),
                    password = passwordField.Text.ToString(),
                    isModerator = false,
                    gender = genderRadioGroup.RadioLabels[genderRadioGroup.SelectedItem].ToString().ToLower(),
                    createdAt = DateTime.Now
                };
                repository.Insert(user);

                MessageBox.Query("Info", "User was registered", "Ok");

                Application.RequestStop();
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Password mismatch", "Ok");
            }
        }
    }
}
