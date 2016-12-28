using System.Linq;
using System.Xml;

namespace QSP.LibraryExtension
{
    public static class Xmls
    {
        public static string ToValidXmlString(this string s)
        {
            return new string(s.Where(c => XmlConvert.IsXmlChar(c)).ToArray());
        }
    }
}