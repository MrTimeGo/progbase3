using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class UserEditionWindow : Window
    {
        Service service;

        User user;

        TextField usernameField;
        TextField passwordField;
        TextField confirmedPasswordField;
        RadioGroup genderRadioGroup;

        public UserEditionWindow(long userId, Service service)
        {
            this.service = service;
            this.user = service.usersRepo.GetById(userId);

            this.Title = "User edition";
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
            Label labelPassword = new Label("New password:")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelUsername) + 1,
                Height = 1
            };
            Label labelConfirmPassword = new Label("Confirm new password:")
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

            usernameField = new TextField(user.username)
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

            genderRadioGroup.SelectedItem = FindIndex(user.gender);

            Button confirmEdition = new Button("Confirm")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(genderRadioGroup) + 2,
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(confirmEdition) + 3,
                Y = Pos.Top(confirmEdition)
            };

            confirmEdition.Clicked += OnConfirmClicked;
            cancel.Clicked += OnCancelClicked;
            

            this.Add(labelUsername, labelPassword, labelConfirmPassword, labelGender,
                usernameField, passwordField, confirmedPasswordField, genderRadioGroup,
                confirmEdition, cancel);
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnConfirmClicked()
        {
            if (passwordField.Text == confirmedPasswordField.Text)
            {
                user.username = usernameField.Text.ToString();
                if (passwordField.Text.ToString() != null)
                {
                    user.password = passwordField.Text.ToString();
                }
                user.gender = genderRadioGroup.RadioLabels[genderRadioGroup.SelectedItem].ToString().ToLower();

                service.usersRepo.Edit(user);

                MessageBox.Query("Info", "User was edited", "Ok");

                Application.RequestStop();
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Password mismatch", "Ok");
            }
        }

        private int FindIndex(string item)
        {
            string[] genders = new string[] { "male", "female", "other" };
            for (int i = 0; i < genders.Length; i++)
            {
                if (item == genders[i])
                    return i;
            }
            return -1;
        }
    }
}
