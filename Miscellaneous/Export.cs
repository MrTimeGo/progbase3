using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using RPC;
using Storage;

namespace Miscellaneous
{
    public static class Export
    {
        public static void Run(string filePath, long postId, RemoteService service)
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
        private static List<Comment> GetCommentsFromPost(long postId, RemoteService service)
        {
            return service.commentsRepo.GetByPostId(postId);
        }
    }
}
