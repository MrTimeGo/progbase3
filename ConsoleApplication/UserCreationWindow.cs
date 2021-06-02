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
        DateField birthDate;
        RadioGroup genderRadioGroup;

        RemoteService service;

        public UserCreationWindow(RemoteService service)
        {
            //this.ColorScheme.Normal = Attribute.Make(Color.Blue, Color.Red);
            this.Title = "Registration";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            this.service = service;

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
            Label labelBirthDate = new Label("Date of birth:")
            {
                X = Pos.Left(labelConfirmPassword),
                Y = Pos.Bottom(labelConfirmPassword) + 1
            };
            Label labelGender = new Label("Gender:")
            {
                X = Pos.Left(labelConfirmPassword),
                Y = Pos.Bottom(labelBirthDate) + 1,
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
            birthDate = new DateField()
            {
                X = Pos.Left(confirmedPasswordField),
                Y = Pos.Top(labelBirthDate)
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

            this.Add(labelUsername, labelPassword, labelConfirmPassword,labelBirthDate, labelGender,
                usernameField, passwordField, confirmedPasswordField,birthDate, genderRadioGroup,
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
                Authentication auth = new Authentication(service);
                string username = this.usernameField.Text.ToString();
                string password = this.passwordField.Text.ToString();
                DateTime birthDate = this.birthDate.Date;
                int gender = this.genderRadioGroup.SelectedItem == 2 ? 0 : this.genderRadioGroup.SelectedItem + 1;
                try
                {
                    auth.Register(username, password, gender, birthDate);

                    Application.RequestStop();
                }
                catch(Exception ex)
                {
                    MessageBox.ErrorQuery("Error", ex.Message, "Ok");
                }

            }
            else
            {
                MessageBox.ErrorQuery("Error", "Password mismatch", "Ok");
            }
        }
    }
}
