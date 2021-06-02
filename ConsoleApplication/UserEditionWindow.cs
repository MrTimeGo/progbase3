using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class UserEditionWindow : Window
    {
        RemoteService service;

        User user;

        TextField usernameField;
        DateField birthDateField;
        RadioGroup genderRadioGroup;

        public UserEditionWindow(long userId, RemoteService service)
        {
            this.service = service;
            this.user = service.usersRepo.GetById(userId);

            this.Title = "User edition";
            this.X = Pos.Percent(10);
            this.Y = Pos.Percent(15);
            this.Width = Dim.Percent(80);
            this.Height = Dim.Percent(70);

            Initialize();
        }
        private void Initialize()
        {
            Label labelUsername = new Label("Username:")
            {
                X = Pos.Percent(15),
                Y = Pos.Percent(50) - 5,
                Height = 1
            };
            Label labelBirthDate = new Label("Date of birth:")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelUsername) + 1
            };
            Label labelGender = new Label("Gender:")
            {
                X = Pos.Left(labelUsername),
                Y = Pos.Bottom(labelBirthDate) + 1,
                Height = 1
            };

            usernameField = new TextField(user.username)
            {
                X = Pos.Percent(60),
                Y = Pos.Top(labelUsername),
                Height = 1,
                Width = Dim.Percent(30)
            };
            birthDateField = new DateField(user.birthDate)
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelBirthDate),
            };
            genderRadioGroup = new RadioGroup(new NStack.ustring[] { "Male", "Female", "Other" })
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelGender),
                SelectedItem = user.gender == 0 ? 2 : user.gender - 1
            };

            Button passwordChange = new Button("Change password")
            {
                X = Pos.Percent(50) - 9,
                Y = Pos.Bottom(genderRadioGroup) + 2,
            };
            Button confirmEdition = new Button("Confirm")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(passwordChange) + 1,
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(confirmEdition) + 3,
                Y = Pos.Top(confirmEdition)
            };

            passwordChange.Clicked += OnPasswordChangeClicked;
            confirmEdition.Clicked += OnConfirmClicked;
            cancel.Clicked += OnCancelClicked;

            this.Add(labelUsername, labelBirthDate, labelGender,
                usernameField, birthDateField, genderRadioGroup,
                passwordChange, confirmEdition, cancel);
        }

        private void OnPasswordChangeClicked()
        {
            ChangePasswordDialog passwordChange = new ChangePasswordDialog();
            Application.Run(passwordChange);

            user.password = passwordChange.password.Text.ToString();
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnConfirmClicked()
        {
            if (usernameField.Text.ToString() == "")
            {
                MessageBox.ErrorQuery("Error", "Username should be not empty", "Ok");
                return;
            }
            if (user.username != usernameField.Text.ToString() && service.usersRepo.UserExists(usernameField.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "Username is busy", "Ok");
                return;
            }
            if ((DateTime.Now - birthDateField.Date).TotalDays < 12 * 365)
            {
                MessageBox.ErrorQuery("Error", "User age should be 12+", "Ok");
                return;
            }
            user.username = usernameField.Text.ToString();
            user.gender = genderRadioGroup.SelectedItem + 1 > 2 ? 0 : genderRadioGroup.SelectedItem + 1;
            user.birthDate = birthDateField.Date;

            service.usersRepo.Edit(user);

            MessageBox.Query("Info", "User was edited", "Ok");

            Application.RequestStop();
        }
        class ChangePasswordDialog : Dialog
        {
            public TextField password;
            public ChangePasswordDialog()
            {
                X = Pos.Center();
                Y = Pos.Center();
                Height = 9;
                Width = 40;
                Title = "Change password";

                Label lableNewPassword = new Label("Enter new password:")
                {
                    X = 3,
                    Y = 1
                };
                password = new TextField()
                {
                    X = Pos.Left(lableNewPassword),
                    Y = Pos.Bottom(lableNewPassword) + 1,
                    Secret = true,
                    Width = 34
                };
                Button confirm = new Button("Confirm")
                {
                    X = Pos.Percent(50) - 10,
                    Y = Pos.Bottom(password) + 2,
                };
                Button cancel = new Button("Cancel")
                {
                    X = Pos.Right(confirm) + 3,
                    Y = Pos.Top(confirm)
                };

                confirm.Clicked += OnConfirmClicked;
                cancel.Clicked += OnCancelClicked;

                this.Add(lableNewPassword, password, confirm, cancel);
            }

            private void OnCancelClicked()
            {
                Application.RequestStop();
            }

            private void OnConfirmClicked()
            {
                if (password.Text.Length < 8)
                {
                    MessageBox.ErrorQuery("Error", "Password length should be more than 7.", "Ok");
                }
                else
                {
                    Application.RequestStop();
                }
            }
        }
    }
}
