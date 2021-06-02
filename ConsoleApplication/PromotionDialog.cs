using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class PromotionDialog : Dialog
    {
        RemoteService service;
        TextField usernameField;
        public PromotionDialog(RemoteService service)
        {
            this.service = service;
            Title = "Promotion dialog";
            X = Pos.Center();
            Y = Pos.Center();
            Width = Dim.Percent(60);
            Height = Dim.Percent(40);
            Initialize();
        }
        private void Initialize()
        {
            Label usernameLabel = new Label("Username:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(50) - 3
            };
            usernameField = new TextField()
            {
                X = Pos.Left(usernameLabel),
                Y = Pos.Bottom(usernameLabel) + 1,
                Width = Dim.Percent(80)
            };
            Button promote = new Button("Promote")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(usernameField) + 1
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(promote) + 3,
                Y = Pos.Top(promote)
            };

            promote.Clicked += OnPromoteClicked;
            cancel.Clicked += OnCancelClicked;

            this.Add(usernameLabel, usernameField, promote, cancel);
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnPromoteClicked()
        {
            string username = usernameField.Text.ToString();
            User user = service.usersRepo.GetByUsername(username);
            if (user == null)
            {
                MessageBox.ErrorQuery("Error", "This user does not exist", "Ok");
            }
            else
            {
                user.isModerator = true;
                service.usersRepo.Edit(user);
                MessageBox.Query("Info", $"User {username} promoted to moderator", "Ok");
                Application.RequestStop();
            }
        }
    }
}
