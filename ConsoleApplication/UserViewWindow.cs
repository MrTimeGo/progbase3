using System;
using Terminal.Gui;

namespace ConsoleApplication
{
    class UserViewWindow : Window
    {
        User user;
        public UserViewWindow(int userId, UsersRepository repo)
        {
            this.user = repo.GetById(userId);
            this.Title = "User Info";

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

            TextField usernameField = new TextField(user.username)
            {
                X = Pos.Percent(60),
                Y = Pos.Top(labelUsername),
                Height = 1,
                Width = Dim.Percent(30),
                ReadOnly = true
            };

            TextField genderField = new TextField(user.gender)
            {
                X = Pos.Left(usernameField),
                Y = Pos.Top(labelGender),
            };

            //TO DO 
            //Button createNewUser = new Button("Register")
            //{
            //    X = Pos.Percent(50) - 12,
            //    Y = Pos.Bottom(genderRadioGroup) + 2,
            //};
            //Button cancel = new Button("Cancel")
            //{
            //    X = Pos.Right(createNewUser) + 3,
            //    Y = Pos.Top(createNewUser)
            //};

            this.Add(labelUsername, labelGender,
                usernameField, genderField
                );
        }
    }
}
