using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Progbase3ClassLib
{
    public static class Export
    {
        public static void Run(string filePath, long postId, Service service)
        {
            List<Comment> comments = GetCommentsFromPost(postId, service);
            WriteCommetsToFile(filePath, comments);
        }
        private static void WriteCommetsToFile(string filePath, List<Comment> comments)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Comment>));
            StreamWriter sw = new StreamWriter(filePath);
            ser.Serialize(sw, comments);
            sw.Close();
        }
        private static List<Comment> GetCommentsFromPost(long postId, Service service)
        {
            return service.commentsRepo.GetByPostId(postId);
        }
    }
}
