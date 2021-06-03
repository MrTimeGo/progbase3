using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RPC
{
    [XmlRoot("response")]
    public class Response<T>
    {
        public bool hasErrors;
        public T returnValue;
    }
}
