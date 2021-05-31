using System;
using System.Collections.Generic;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{ 
    class PostsListWindow : Window
    {
        Service service;
        User loggedUser;

        int currentPage;
        int totalPages;
        string searchKeyword;
        long userId;

        TextField searchField;
        ListView page;
        Label notFoundLabel;
        Button nextPage;
        Button prevPage;
        TextField bottomPageCounter;
        Label bottomAllPage;
        Label pageNumber;
        public PostsListWindow(long userId, Service service, User loggedUser)
        {
            this.service = service;
            this.loggedUser = loggedUser;

            this.userId = userId;
            this.currentPage = 1;
            this.searchKeyword = "";
            this.totalPages = service.postsRepo.GetTotalPages(searchKeyword, userId);

            Initialize();
        }
        private void Initialize()
        {
            this.Title = "Posts";
            this.X = Pos.Percent(10);
            this.Y = Pos.Percent(10);
            this.Width = Dim.Percent(80);
            this.Height = Dim.Percent(80);
            pageNumber = new Label()
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Height = 1,
                Width = 10
            };
            searchField = new TextField()
            {
                X = Pos.Percent(60),
                Y = Pos.Top(pageNumber),
                Width = Dim.Percent(30)
            };
            page = new ListView()
            {
                X = Pos.Percent(50) - 40,
                Y = Pos.Percent(50) - 5,
                Width = 80,
                Height = 10
            };
            notFoundLabel = new Label("Found no posts")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Visible = false
            };
            prevPage = new Button("<-")
            {
                X = Pos.Right(page) - 20,
                Y = Pos.Bottom(page) + 2,
            };
            bottomPageCounter = new TextField()
            {
                X = Pos.Right(prevPage) + 1,
                Y = Pos.Top(prevPage),
                Width = 2,
                Height = Dim.Height(prevPage)
            };
            bottomAllPage = new Label()
            {
                X = Pos.Right(bottomPageCounter) + 1,
                Y = Pos.Top(bottomPageCounter),
                Height = Dim.Height(bottomPageCounter)
            };
            nextPage = new Button("->")
            {
                X = Pos.Right(bottomAllPage) + 1,
                Y = Pos.Top(bottomAllPage),
            };

            Button exit = new Button("Exit")
            {
                X = Pos.Left(bottomPageCounter),
                Y = Pos.Bottom(bottomPageCounter) + 1,
            };
            UpdateInfo();

            searchField.TextChanging += OnTextChanging;
            page.OpenSelectedItem += OnSeclectedItem;
            this.Enter += UpdateInfo;

            prevPage.Clicked += OnPrevPageClicked;
            nextPage.Clicked += OnNextPageClicked;
            bottomPageCounter.KeyDown += OnPageCounterClicked;

            exit.Clicked += OnExitClicked;

            this.Add(pageNumber, searchField, page, notFoundLabel,
                prevPage, bottomPageCounter, bottomAllPage, nextPage,
                exit);
        }

        private void UpdateInfo(FocusEventArgs obj)
        {
            UpdateInfo();
        }

        private void OnTextChanging(TextChangingEventArgs obj)
        {
            searchKeyword = obj.NewText.ToString();
            currentPage = 1;
            UpdateInfo();
        }

        private void OnSeclectedItem(ListViewItemEventArgs obj)
        {
            Post post = (Post)obj.Value;
            Window selectedInfo = new PostViewWindow(post.id, service, loggedUser);
            Application.Top.Add(selectedInfo);
            Application.RequestStop();
            Application.Run();

            UpdateInfo();
        }

        private void OnExitClicked()
        {
            Application.Top.Remove(this);
        }
        private void UpdateInfo()
        {
            totalPages = service.postsRepo.GetTotalPages(searchKeyword, userId);
            if (totalPages == 0)
            {
                bottomPageCounter.Text = "0";
                bottomAllPage.Text = "/0";
                prevPage.Visible = false;
                nextPage.Visible = false;
                page.Visible = false;
                notFoundLabel.Visible = true;
                Application.Refresh();
                return;
            }
            page.Visible = true;
            notFoundLabel.Visible = false;
            prevPage.Visible = currentPage != 1;
            nextPage.Visible = currentPage != totalPages;

            pageNumber.Text = $"Page {this.currentPage}";
            bottomPageCounter.Text = currentPage.ToString();
            bottomAllPage.Text = $"/{totalPages}";
            Application.Refresh();

            List<Post> source = service.postsRepo.GetPage(currentPage, searchKeyword, userId);
            page.SetSource(source);
        }

        private void OnPageCounterClicked(KeyEventEventArgs obj)
        {
            if (obj.KeyEvent.Key == Key.Enter)
            {
                if (!int.TryParse(bottomPageCounter.Text.ToString(), out int number))
                {
                    bottomPageCounter.Text = string.Empty;
                    return;
                }
                if (number > totalPages || number < 1)
                {
                    bottomPageCounter.Text = string.Empty;
                    return;
                }
                currentPage = number;
                UpdateInfo();
            }
        }

        private void OnNextPageClicked()
        {
            currentPage++;
            UpdateInfo();
        }
        private void OnPrevPageClicked()
        {
            currentPage--;
            UpdateInfo();
        }
    }
}
