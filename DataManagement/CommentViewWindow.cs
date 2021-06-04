using System;
using Terminal.Gui;
using Storage;
using RPC;

namespace DataManagement
{
    class CommentViewWindow : Window
    {
        Comment comment;
        RemoteService service;
        User loggedUser;
        User author;

        Label authorName;
        Label postPreview;
        Label text;
        public CommentViewWindow(long commentId, RemoteService service, User loggedUser)
        {
            this.comment = service.commentsRepo.GetById(commentId);
            this.service = service;
            this.loggedUser = loggedUser;

            this.Title = "Comment";
            this.X = Pos.Percent(20);
            this.Y = Pos.Percent(20);
            this.Width = Dim.Percent(60);
            this.Height = Dim.Percent(60);

            Initialize();
        }
        private void Initialize()
        {
            author = service.usersRepo.GetById(comment.authorId);
            string authorUsername;
            if (author == null)
            {
                authorUsername = "DELETED";
            }
            else
            {
                authorUsername = author.username;
            }
            authorName = new Label(authorUsername)
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
            };

            Label creationTime = new Label(comment.publishTime.ToShortDateString())
            {
                X = Pos.Percent(70),
                Y = Pos.Top(authorName)
            };

            Window inputWindow = new Window()
            {
                X = Pos.Left(authorName),
                Y = Pos.Bottom(authorName) + 1,
                Width = Dim.Percent(80),
                Height = Dim.Fill(8)
            };

            text = new Label()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Text = comment.text
            };

            inputWindow.Add(text);

            Post post = service.postsRepo.GetById(comment.postId);
            Label postLabel = new Label("In post:")
            { 
                X = Pos.Left(inputWindow),
                Y = Pos.Bottom(inputWindow) + 1,
            };
            CheckBox pinnedCheckbox = new CheckBox("Pinned")
            {
                X = Pos.Percent(90) - 8,
                Y = Pos.Top(postLabel),
                Checked = comment.isPinned,
                Visible = post.authorId == loggedUser.id
            };
           
            string postTitle = "";
            if (post == null)
            {
                postTitle = "DELETED";
            }
            else
            {
                string shortTitle = post.title.Length <= 30 ? post.title : post.title.Substring(0, 27) + "...";
                postTitle = $"[{post.id}] {shortTitle}";
            }
            postPreview = new Label(postTitle)
            {
                X = Pos.Left(postLabel),
                Y = Pos.Bottom(postLabel) + 1,
            };

            Button edit = new Button("Edit")
            {
                X = Pos.Percent(50) - 16,
                Y = Pos.Bottom(postPreview) + 2,
            };
            Button delete = new Button("Delete")
            {
                X = Pos.Right(edit) + 3,
                Y = Pos.Top(edit)
            };
            Button exit = new Button("Exit")
            {
                X = Pos.Right(delete) + 3,
                Y = Pos.Top(edit)
            };

            if (!loggedUser.isModerator && loggedUser.id != comment.authorId)
            {
                edit.Visible = false;
                delete.Visible = false;
            }

            authorName.Clicked += OnAuthorClicked;
            postPreview.Clicked += OnPostPreviewClicked;
            pinnedCheckbox.Toggled += OnCheckBoxToggled;

            edit.Clicked += OnEditClicked;
            delete.Clicked += OnDeleteClicked;
            exit.Clicked += OnExitClicked;

            this.Add(authorName, creationTime,
                inputWindow, postLabel, pinnedCheckbox, postPreview,
                edit, delete, exit);
        }

        private void OnCheckBoxToggled(bool obj)
        {
            comment.isPinned = !obj;
            service.commentsRepo.EditById(comment);
        }

        private void OnPostPreviewClicked()
        {
            Window postInfo = new PostViewWindow(comment.postId, service, loggedUser);
            Application.RequestStop();
            Application.Top.Add(postInfo);
            Application.RequestStop();
            Application.Run();

            UpdateWindow();
        }

        private void OnAuthorClicked()
        {
            if (author != null)
            {
                Window userInfo = new UserViewWindow(comment.authorId, service, loggedUser);
                Application.RequestStop();
                Application.Top.Add(userInfo);
                Application.RequestStop();
                Application.Run();

                UpdateWindow();
            }
        }

        private void OnExitClicked()
        {
            Application.RequestStop();
        }

        private void OnDeleteClicked()
        {
            int result = MessageBox.Query("Info", "Are you sure, that you wand to delete comment", "Yes", "No");
            if (result == 0)
            {
                service.commentsRepo.DeleteById(comment.id);
                MessageBox.Query("Info", "Comment was deleted", "Ok");
                Application.RequestStop();
            }
        }

        private void OnEditClicked()
        {
            Window edition = new CommentEditionWindow(comment.id, service);
            Application.Run(edition);

            UpdateWindow();
        }
        private void UpdateWindow()
        {
            comment = service.commentsRepo.GetById(comment.id);

            text.Text = comment.text;
            authorName.Text = service.usersRepo.GetById(comment.authorId).username;
            postPreview.Text = service.postsRepo.GetById(comment.postId).ToString();
        }
    }
}
