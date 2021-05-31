using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Progbase3ClassLib
{
    [XmlRoot("response")]
    public class Response<T>
    {
        public bool isError;
        public bool isRequestSuccessfull;
        public T returnValue;
    }
}
