using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Progbase3ClassLib
{
    [XmlRoot("request")]
    public class Request
    {
        public string methodName;
        public List<string> parameters;
    }
}
