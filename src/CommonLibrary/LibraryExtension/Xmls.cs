using System.Linq;
using System.Xml;

namespace CommonLibrary.LibraryExtension
{
    public static class Xmls
    {
        public static string RemoveIllegalXmlChar(this string s)
        {
            return new string(s.CastStr().Where(c => IsValidXmlChar(c)).ToArray());
        }

        public static bool IsValidXmlChar(this char c)
        {
            try
            {
                XmlConvert.VerifyXmlChars(new string(c, 1));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}