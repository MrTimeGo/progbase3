using System;
using System.Collections.Generic;
using Terminal.Gui;
using RPC;
using Storage;

namespace DataManagement
{
    class PostViewWindow : Window
    {
        Post post;
        RemoteService service;
        User loggedUser;
        User author;

        Label authorName;
        TextView text;
        Label titleLabel;
        Label pinnedCommentLabel;
        Label pinnedCommentPreview;
        public PostViewWindow(long postId, RemoteService service, User loggedUser)
        {
            this.post = service.postsRepo.GetById(postId);
            this.service = service;
            this.loggedUser = loggedUser;

            this.Title = "Post";
            Y = Pos.Percent(0) + 1;
            this.Width = Dim.Fill();
            this.Height = Dim.Fill();

            Initialize();
        }
        private void Initialize()
        {
            titleLabel = new Label(post.title)
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Width = Dim.Percent(80),
                Height = 3
            };
            author = service.usersRepo.GetById(post.authorId);
            string authorUsername;
            if (author == null)
            {
                authorUsername = "DELETED";
            }
            else
            {
                authorUsername = author.username;
            }

            authorName = new Label($"By {authorUsername}")
            {
                X = Pos.Left(titleLabel),
                Y = Pos.Bottom(titleLabel) + 1
            };

            Label creationTime = new Label(post.publishTime.ToShortDateString())
            {
                X = Pos.Percent(70),
                Y = Pos.Top(authorName)
            };

            Window inputWindow = new Window()
            {
                X = Pos.Left(titleLabel),
                Y = Pos.Bottom(authorName),
                Width = Dim.Percent(80),
                Height = Dim.Fill(10)
            };

            text = new TextView()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
                Text = post.text
            };
            Comment pinnedComment = service.commentsRepo.GetPinnedComment(post.id);
            pinnedCommentLabel = new Label("Pinned comment:")
            {
                X = Pos.Left(inputWindow),
                Y = Pos.Bottom(inputWindow) + 1,
            };
            pinnedCommentPreview = new Label()
            {
                X = Pos.Left(pinnedCommentLabel),
                Y = Pos.Bottom(pinnedCommentLabel) + 1,
            };
            if (pinnedComment == null)
            {
                pinnedCommentLabel.Visible = false;
                pinnedCommentPreview.Visible = false;
            }
            else
            {
                pinnedCommentPreview.Text = pinnedComment.text;
            }

            inputWindow.Add(text);

            Button allComments = new Button("View comments")
            {
                X = Pos.Percent(50) - 19,
                Y = Pos.Bottom(inputWindow) + 6
            };
            Button newComment = new Button("Write new comment")
            {
                X = Pos.Right(allComments) + 3,
                Y = Pos.Top(allComments)
            };
            Button edit = new Button("Edit")
            {
                X = Pos.Percent(50) - 16,
                Y = Pos.Bottom(allComments) + 1,
            };
            Button exit = new Button("Exit")
            {
                X = Pos.Right(edit) + 3,
                Y = Pos.Top(edit)
            };
            Button delete = new Button("Delete")
            {
                X = Pos.Right(exit) + 3,
                Y = Pos.Top(edit)
            };


            if (!loggedUser.isModerator && loggedUser.id != post.authorId)
            {
                edit.Visible = false;
                delete.Visible = false;
            }

            allComments.Clicked += OnAllCommentsClicked;
            newComment.Clicked += OnNewCommentClicked;
            authorName.Clicked += OnAuthorClicked;

            edit.Clicked += OnEditClicked;
            delete.Clicked += OnDeleteClicked;
            exit.Clicked += OnExitClicked;
            this.Enter += UpdateInfo;

            this.Add(titleLabel, authorName, creationTime,
                inputWindow,
                pinnedCommentLabel, pinnedCommentPreview,
                allComments, newComment,
                edit, exit, delete);
        }

        private void UpdateInfo(FocusEventArgs obj)
        {
            UpdateWindow();
        }

        private void OnNewCommentClicked()
        {
            CommentCreationWindow win = new CommentCreationWindow(service, post.id, loggedUser);
            Application.Run(win);
        }

        private void OnAuthorClicked()
        {
            if (author != null)
            {
                Window infoWindow = new UserViewWindow(post.authorId, service, loggedUser);
                Application.Top.Add(infoWindow);
                Application.RequestStop();
                Application.Run();

                UpdateWindow();
            }
        }

        private void OnExitClicked()
        {
            Application.Top.Remove(this);
        }

        private void OnDeleteClicked()
        {
            int result = MessageBox.Query("Info", "Are you sure, that you wand to delete post", "Yes", "No");
            if (result == 0)
            {
                service.postsRepo.DeleteById(post.id);
                MessageBox.Query("Info", "Post was deleted", "Ok");
                Application.Top.Remove(this);
            }
        }

        private void OnEditClicked()
        {
            Window editionWindow = new PostEditionWindow(post.id, service);
            Application.Run(editionWindow);

            UpdateWindow();
        }

        private void OnAllCommentsClicked()
        {
            Window listWindow = new CommentsListWindow(false, post.id, service, loggedUser);
            Application.Top.Add(listWindow);
            Application.RequestStop();
            Application.Run();

            UpdateWindow();
        }
        private void UpdateWindow()
        {
            post = service.postsRepo.GetById(post.id);
            authorName.Text = service.usersRepo.GetById(post.authorId).username;
            titleLabel.Text = post.title;
            text.Text = post.text;
            Comment pinnedComment = service.commentsRepo.GetPinnedComment(post.id);
            if (pinnedComment != null)
            {
                pinnedCommentPreview.Text = pinnedComment.text;
                pinnedCommentLabel.Visible = true;
                pinnedCommentPreview.Visible = true;
            }
            else
            {
                pinnedCommentLabel.Visible = false;
                pinnedCommentPreview.Visible = false;
            }
            Application.Refresh();
        }
    }
}
