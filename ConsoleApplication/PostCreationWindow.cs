using System;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class PostCreationWindow : Window
    {
        RemoteService service;
        User loggedUser;

        TextField titleField;
        TextView plainTextView;

        public PostCreationWindow(RemoteService service, User loggedUser)
        {
            this.Title = "New post";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            this.service = service;
            this.loggedUser = loggedUser;
            Initialize();
        }
        private void Initialize()
        {
            Label labelTitle = new Label("Title:")
            {
                X = Pos.Percent(10),
                Y = 1,
                Height = 1
            };
            titleField = new TextField()
            {
                X = Pos.Left(labelTitle),
                Y = Pos.Bottom(labelTitle) + 1,
                Width = Dim.Percent(80),
                Height = 1
            };
            Label labelPlainText = new Label("Text:")
            {
                X = Pos.Left(labelTitle),
                Y = Pos.Bottom(titleField) + 1,
                Height = 1
            };

            Window inputWin = new Window()
            {
                X = Pos.Left(labelTitle),
                Y = Pos.Bottom(labelPlainText) + 1,
                Width = Dim.Percent(80),
                Height = Dim.Fill(4),
            };
            plainTextView = new TextView()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            inputWin.Add(plainTextView);

            Button createNewPost = new Button("Publish")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(inputWin) + 2,
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(createNewPost) + 3,
                Y = Pos.Top(createNewPost)
            };

            createNewPost.Clicked += OnCreateNewPostClicked;
            cancel.Clicked += OnCancelClicked;

            this.Add(labelTitle, titleField,
                labelPlainText, inputWin,
                createNewPost, cancel);
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnCreateNewPostClicked()
        {
            if (titleField.Text.ToString() == "")
            {
                MessageBox.ErrorQuery("Error", "Post title should be not empty", "Ok");
                return;
            }
            if (plainTextView.Text.ToString() == "")
            {
                MessageBox.ErrorQuery("Error", "Post text should be not empty", "Ok");
                return;
            }
            Post post = new Post()
            {
                authorId = loggedUser.id,
                title = titleField.Text.ToString(),
                text = plainTextView.Text.ToString(),
                publishTime = DateTime.Now
            };
            service.postsRepo.Insert(post);

            MessageBox.Query("Info", "Post was published", "Ok");

            Application.RequestStop();
        }
    }
}
