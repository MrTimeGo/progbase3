using System;
using Terminal.Gui;
using Progbase3ClassLib;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApplication
{
    class MainWindow : Window
    {
        Service service;

        Label logginedUserLabel;
        User logginedUser;

        Button toModerator;
        public MainWindow(Service service, User logginedUser)
        {
            this.service = service;
            this.logginedUser = logginedUser;
            this.Title = "Main Window";
            Y = Pos.Percent(0) + 1;
            Initialize();
        }
        private void Initialize()
        {
            Label labelWelcome = new Label("Welcome to social network.\nHere you can find posts for different topics, comment them or write your own discussion.")
            {
                X = Pos.Center(),
                Y = Pos.Percent(30),
                Height = 3,
                TextAlignment = TextAlignment.Centered
            };
            logginedUserLabel = new Label()
            {
                X = Pos.Center(),
                Y = Pos.Bottom(labelWelcome) + 1,
            };
            Button myProfile = new Button("My profile")
            { 
                X = Pos.Center(),
                Y = Pos.Bottom(logginedUserLabel) + 2
            };
            Button browsePosts = new Button("Browse posts")
            {
                X = Pos.Percent(50) - 16,
                Y = Pos.Bottom(myProfile) + 1,
            };
            Button writePost = new Button("Write post")
            {
                X = Pos.Right(browsePosts) + 3,
                Y = Pos.Top(browsePosts)
            };
            Button logout = new Button("Log out")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(browsePosts) + 1
            };
            toModerator = new Button("Promote to moderator")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(logout) + 2,
                Visible = false,
            };

            myProfile.Clicked += OnMyProfileClicked;
            browsePosts.Clicked += OnBrowsePostsClicked;
            writePost.Clicked += OnWritePostClicked;
            logout.Clicked += OnLogoutClickd;
            toModerator.Clicked += OnToModeratorClicked;

            this.Add(labelWelcome, logginedUserLabel, myProfile, browsePosts, writePost, logout, toModerator);
            if (logginedUser == null)
            {
                RunLoginDialog();
            }
            else
            {
                logginedUserLabel.Text = $"Logged in as {logginedUser.username}";
            }
        }

        private void OnToModeratorClicked()
        {
            PromotionDialog promotion = new PromotionDialog(service);
            Application.Run(promotion);
        }

        private void OnLogoutClickd()
        {
            logginedUser = null;
            RunLoginDialog();
        }

        private void OnWritePostClicked()
        {
            PostCreationWindow postCreation = new PostCreationWindow(service, logginedUser);
            Application.Run(postCreation);
        }
        private void OnBrowsePostsClicked()
        {
            PostsListWindow postList = new PostsListWindow(-1, service, logginedUser);
            Application.Top.Add(postList);
            Application.RequestStop();
            Application.Run();
            
        }

        private void OnMyProfileClicked()
        {
            UserViewWindow userInfo = new UserViewWindow(logginedUser.id, service, logginedUser);
            Application.Top.Add(userInfo);
            Application.RequestStop();
            Application.Run();
        }

        private void RunLoginDialog()
        {
            LoginDialog loginDialog = new LoginDialog(service);
            Application.Run(loginDialog);
            logginedUser = loginDialog.loggedInUser;
            logginedUserLabel.Text = $"Logged in as {logginedUser.username}";
            logginedUserLabel.X = Pos.Center();
            if (logginedUser.isModerator)
            {
                toModerator.Visible = true;
                Application.Refresh();
            }
        }
    }
}
