using System;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ScottPlot;
using System.IO;

namespace Progbase3ClassLib
{
    public static class ReportGenerator
    {
        public static void GenerateNewReport(long postId, DateTime dateFrom, DateTime dateTo, Service service, string saveToPath)
        {
            DeleteOldReportIfExists(saveToPath);
            const string sampleFilePath = @"./../data/ReportSample.docx";
            ReportInfo reportInfo = GetReportInfo(postId, service, dateFrom, dateTo);
            ZipFile.ExtractToDirectory(sampleFilePath, $"{saveToPath}Report");
            GenerateGraphics(reportInfo, service, saveToPath);
            XElement root = XElement.Load($"{saveToPath}Report/word/document.xml");
            FindAndReplace(root, reportInfo);
            root.Save($"{saveToPath}Report/word/document.xml");

            ZipFile.CreateFromDirectory($"{saveToPath}Report", $"{saveToPath}Report.docx");
            Directory.Delete($"{saveToPath}Report", true);
        }
        static void DeleteOldReportIfExists(string saveToPath)
        {
            if (File.Exists(saveToPath + "Report.docx"))
            {
                File.Delete(saveToPath + "Report.docx");
            }
        }
        static void FindAndReplace(XElement node, ReportInfo reportInfo)
        {
            if (node.FirstNode != null
                && node.FirstNode.NodeType == XmlNodeType.Text)
            {
                switch (node.Value)
                {
                    case "{title}": node.Value =reportInfo.title; break;
                    case "{authorName}": node.Value = reportInfo.authorName; break;
                    case "{text}": node.Value = reportInfo.text; break;
                    case "{numberOfComments}": node.Value = reportInfo.numberOfComments.ToString(); break;
                    case "{userName}": node.Value = reportInfo.userName; break;
                    case "{dateFrom}": node.Value = reportInfo.dateFrom.ToString(); break;
                    case "{dateTo}": node.Value = reportInfo.dateTo.ToString(); break;
                    case "{":
                    case "}":
                        node.Value = ""; break;
                }
            }

            foreach (XElement el in node.Elements())
            {
                FindAndReplace(el, reportInfo);
            }
        }

        struct ReportInfo
        {
            public long postId;
            public string title;
            public string authorName;
            public string text;
            public int numberOfComments;
            public string userName;
            public DateTime dateFrom;
            public DateTime dateTo;
        }
        private static ReportInfo GetReportInfo(long postId, Service service, DateTime dateFrom, DateTime dateTo)
        {
            Post post = service.postsRepo.GetById(postId);
            ReportInfo ri = new ReportInfo()
            {
                postId = postId,
                title = post.title,
                authorName = service.usersRepo.GetById(postId).username,
                text = post.text,
                numberOfComments = service.commentsRepo.GetByPostId(postId).Count,
                userName = GetUserThatWriteMostComments(postId, service),
                dateFrom = dateFrom,
                dateTo = dateTo
            };
            return ri;
        }
        private static string GetUserThatWriteMostComments(long postId, Service service)
        {
            List<Comment> comments = service.commentsRepo.GetByPostId(postId);
            if (comments.Count == 0)
            {
                return "None";
            }
            Dictionary<long, int> dict = new Dictionary<long, int>();
            foreach (Comment comment in comments)
            {
                dict.Add(comment.authorId, 0);
            }
            foreach(Comment comment in comments)
            {
                dict.TryGetValue(comment.authorId, out int counter);
                dict.Remove(comment.authorId);
                dict.Add(comment.authorId, counter + 1);
            }
            int[] valuesArray = new int[dict.Values.Count];
            dict.Values.CopyTo(valuesArray, 0);
            int index = FindMax(valuesArray);
            long[] keysArray = new long[dict.Keys.Count];
            dict.Keys.CopyTo(keysArray, 0);
            return service.usersRepo.GetById(keysArray[index]).username;
        }
        private static int FindMax(int[] array)
        {
            int max = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[max] < array[i])
                {
                    max = i;
                }
            }
            return max;
        }

        private static void GenerateGraphics(ReportInfo reportInfo, Service service, string saveToPath)
        {
            var plt = new Plot(800, 400);
            double[] xs = MakeXAxis();
            double[] ys = MakeYAxis(reportInfo, service);

            plt.PlotBar(xs, ys);

            string[] labels = GetLabels(reportInfo);
            plt.XTicks(xs, labels);

            plt.SaveFig(saveToPath + @"Report/word/media/image1.png");
        }
        private static string[] GetLabels(ReportInfo reportInfo)
        {
            string[] labels = new string[40];
            TimeSpan scale = (reportInfo.dateTo - reportInfo.dateFrom) / 10;
            for (int i = 0; i < labels.Length; i += 4)
            {
                labels[i] = $"{(reportInfo.dateFrom + i/4 * scale).ToShortDateString()}\n{(reportInfo.dateFrom + i/4 * scale).ToShortTimeString()}";
            }
            return labels;
        }
        private static double[] MakeYAxis(ReportInfo reportInfo, Service service)
        {
            double[] ys = new double[40];
            TimeSpan scale = (reportInfo.dateTo - reportInfo.dateFrom) / 10;
            for (int i = 0; i < ys.Length; i += 4)
            {
                ys[i] = service.commentsRepo.GetCommentCountBasedOnTimeSpan(reportInfo.postId, reportInfo.dateFrom + i/4 * scale, reportInfo.dateFrom + (i/4 + 1) * scale);
            }
            return ys;
        }
        private static double[] MakeXAxis()
        {
            double[] xs = new double[40];
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = i+1;
            }
            return xs;
        }
    }
}
