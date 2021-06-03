using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace RPC
{
    public static class Serializer
    {
        public static Request DeserializeRequest(string xmlRequest)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Request));
            StringReader reader = new StringReader(xmlRequest);
            Request request = (Request)ser.Deserialize(reader);
            reader.Close();
            return request;
        }
        public static string SerializeRequest(Request request)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Request));
            StringWriter writer = new StringWriter();
            ser.Serialize(writer, request);
            writer.Close();
            return writer.ToString();
        }
        public static Response<T> DeserializeResponse<T>(string xmlResponse)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Response<T>));
            StringReader reader = new StringReader(xmlResponse);
            Response<T> response = (Response<T>)ser.Deserialize(reader);
            reader.Close();
            return response;
        }
        public static string SerializeResponse<T>(Response<T> response)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Response<T>));
            StringWriter writer = new StringWriter();
            ser.Serialize(writer, response);
            writer.Close();
            return writer.ToString();
        }
    }
}
