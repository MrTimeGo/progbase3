using System;
using Terminal.Gui;

namespace ConsoleApplication
{
    class PostEditionWindow : Window
    {
        Service service;
        Post post;

        TextField titleField;
        TextView plainTextView;
        public PostEditionWindow(long postId, Service service)
        {
            this.Title = "Post edition";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            this.service = service;
            this.post = service.postsRepo.GetById(postId);
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
            titleField = new TextField(post.title)
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
                Text = post.text
            };

            inputWin.Add(plainTextView);

            Button confirmEdition = new Button("Confirm")
            {
                X = Pos.Percent(50) - 12,
                Y = Pos.Bottom(inputWin) + 2,
            };
            Button cancel = new Button("Cancel")
            {
                X = Pos.Right(confirmEdition) + 3,
                Y = Pos.Top(confirmEdition)
            };

            confirmEdition.Clicked += OnConfirmClicked;
            cancel.Clicked += OnCancelClicked;

            this.Add(labelTitle, titleField,
                labelPlainText, inputWin,
                confirmEdition, cancel);
        }

        private void OnCancelClicked()
        {
            Application.RequestStop();
        }

        private void OnConfirmClicked()
        {
            post.title = titleField.Text.ToString();
            post.text = plainTextView.Text.ToString();

            service.postsRepo.EditById(post);

            MessageBox.Query("Info", "Post was edited", "Ok");
            Application.RequestStop();
        }
    }
}
