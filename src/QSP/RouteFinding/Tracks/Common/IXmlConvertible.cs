using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface IXmlConvertible
    {
        XDocument ToXml();
        void LoadFromXml(XDocument doc);        
    }
}
