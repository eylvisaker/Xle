using System;
using System.Xml.Linq;

namespace ERY.Xle.Data
{
    static class XmlExtensions
    {
        public static T GetOptionalAttribute<T>(this XElement node, string attrib, T defaultValue)
        {
            if (node.Attribute(attrib) != null)
                return (T)Convert.ChangeType(node.Attribute(attrib).Value, typeof(T));
            else
                return defaultValue;
        }

    }
}