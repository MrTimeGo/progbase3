using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace DataManagement
{
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
