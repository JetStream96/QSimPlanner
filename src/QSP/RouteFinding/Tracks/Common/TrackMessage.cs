using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackMessage
    {
        // Should be able to load the return value of LoadFromXml(XDocument)
        // method correctly.
        public abstract XDocument ToXml();
    }
}
