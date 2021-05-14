using System;
using System.Collections.Generic;
using Terminal.Gui;
using Progbase3ClassLib;

namespace ConsoleApplication
{
    class CommentsListWindow : Window
    {
        Service service;

        List<Comment> comments;
        int currentPage;
        ListView page;
        Button nextPage;
        Button prevPage;
        TextField bottomPageCounter;
        Label pageNumber;
        public CommentsListWindow(List<Comment> comments, Service service)
        {
            this.comments = comments;
            this.service = service;

            this.Title = "Comments";
            this.X = Pos.Percent(10);
            this.Y = Pos.Percent(10);
            this.Width = Dim.Percent(80);
            this.Height = Dim.Percent(80);

            this.currentPage = 1;

            Initialize();
        }
        private void Initialize()
        {
            Comment pinned = SearchForPinnedComment();
            if (pinned != null)
            {
                Label pinnedInfo = new Label("Pinned comment:")
                {
                    X = Pos.Percent(10),
                    Y = Pos.Percent(10),
                    Height = 1
                };
                Label pinnedLabel = new Label(pinned.ToString())
                {
                    X = Pos.Percent(10),
                    Y = Pos.Bottom(pinnedInfo) + 1,
                    Height = 1
                };

                this.Add(pinnedInfo, pinnedLabel);

                pageNumber = new Label($"Page {this.currentPage}")
                {
                    X = Pos.Percent(10),
                    Y = Pos.Bottom(pinnedLabel) + 2,
                    Height = 1,
                    Width = 10
                };
            }
            else
            {
                pageNumber = new Label($"Page {this.currentPage}")
                {
                    X = Pos.Percent(10),
                    Y = Pos.Percent(10),
                    Height = 1,
                    Width = 10
                };
            }

            
            page = new ListView()
            {
                X = Pos.Percent(50) - 40,
                Y = Pos.Percent(50) - 5,
                Width = 80,
                Height = 10
            };
            prevPage = new Button("<-")
            {
                X = Pos.Right(page) - 20,
                Y = Pos.Bottom(page) + 2,
            };
            bottomPageCounter = new TextField(currentPage.ToString())
            {
                X = Pos.Right(prevPage) + 1,
                Y = Pos.Top(prevPage),
                Width = 2,
                Height = Dim.Height(prevPage)
            };
            Label bottomAllPage = new Label($"/{GetNumberOfPages()}")
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


            prevPage.Visible = false;
            if (GetNumberOfPages() == 1)
            {
                nextPage.Visible = false;
            }
            prevPage.SetNeedsDisplay();
            nextPage.SetNeedsDisplay();

            page.OpenSelectedItem += OnSelectedItem;

            prevPage.Clicked += OnPrevPageClicked;
            nextPage.Clicked += OnNextPageClicked;
            bottomPageCounter.KeyDown += OnPageCounterClicked;

            exit.Clicked += OnExitClicked;

            this.Add(pageNumber, page,
                prevPage, bottomPageCounter, bottomAllPage, nextPage,
                exit);

            UpdateListView();
        }

        private void OnSelectedItem(ListViewItemEventArgs obj)
        {
            Comment comment = (Comment)obj.Value;
            Window selectedComment = new CommentViewWindow(comment.id, service);
            Application.Run(selectedComment);

            comments[obj.Item] = service.commentsRepo.GetById(comment.id);
            page.SetSource(GetPage(currentPage));
        }

        private void OnExitClicked()
        {
            Application.RequestStop();
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
                if (number > GetNumberOfPages() || number < 1)
                {
                    bottomPageCounter.Text = string.Empty;
                    return;
                }
                currentPage = number;
                pageNumber.Text = $"Page {currentPage}";
                prevPage.Visible = currentPage != 1;
                nextPage.Visible = currentPage != GetNumberOfPages();
                nextPage.Redraw(nextPage.Frame);
                prevPage.Redraw(prevPage.Frame);
                UpdateListView();
            }
        }

        private void OnNextPageClicked()
        {
            currentPage++;
            pageNumber.Text = $"Page {currentPage}";
            bottomPageCounter.Text = currentPage.ToString();
            UpdateListView();
            prevPage.Visible = currentPage != 1;
            nextPage.Visible = currentPage != GetNumberOfPages();
            nextPage.Redraw(nextPage.Frame);
            prevPage.Redraw(prevPage.Frame);
        }
        private void OnPrevPageClicked()
        {
            currentPage--;
            pageNumber.Text = $"Page {currentPage}";
            bottomPageCounter.Text = currentPage.ToString();
            UpdateListView();
            prevPage.Visible = currentPage != 1;
            nextPage.Visible = currentPage != GetNumberOfPages();
        }

        private void UpdateListView()
        {
            page.SetSource(GetPage(currentPage));
        }
        private List<Comment> GetPage(int n)
        {
            const int pageSize = 10;
            List<Comment> page = new List<Comment>();
            for (int i = (n - 1) * pageSize; i < Math.Min(n * pageSize, comments.Count); i++)
            {
                page.Add(comments[i]);
            }
            return page;
        }
        private int GetNumberOfPages()
        {
            const int pageSize = 10;
            return (int)Math.Ceiling(comments.Count / (double)pageSize);
        }
        private Comment SearchForPinnedComment()
        {
            foreach(Comment comment in comments)
            {
                if (comment.isPinned)
                {
                    return comment;
                }
            }
            return null;
        }
    }
}
