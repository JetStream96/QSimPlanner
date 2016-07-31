using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackMessage : IXmlConvertible 
    {
        public abstract string TrackSystem { get; }

        // The root node of XDocument should contain a "TrackSystem" node
        // with value identical to TrackSystem property. 
        // This is used to distinguish between Nats/Pacots/Ausots.
        public abstract void LoadFromXml(XDocument doc);

        // Should be able to load the return value of LoadFromXml(XDocument)
        // method correctly.
        public abstract XDocument ToXml();
    }
}
