using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using RPC;
using Storage;

namespace Miscellaneous
{
    public class Import
    {
        public static void Run(string filePath, RemoteService service)
        {
            ValidateFile(filePath);
            List<Comment> comments = GetComments(filePath);
            WriteToDataBase(service, comments);
        }
        private static void WriteToDataBase(RemoteService service, List<Comment> comments)
        {
            foreach(Comment comment in comments)
            {
                service.commentsRepo.Insert(comment);
            }
        }
        private static List<Comment> GetComments(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            XmlSerializer ser = new XmlSerializer(typeof(List<Comment>));
            List<Comment> comments;
            try
            {
                comments = (List<Comment>)ser.Deserialize(sr);
            }
            catch
            {
                throw new Exception("Cannot import file");
            }
            sr.Close();
            return comments;
        }
        private static void ValidateFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File does not exist");
            }
        }
    }
}
