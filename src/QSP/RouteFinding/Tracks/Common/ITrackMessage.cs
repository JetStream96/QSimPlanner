using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Common
{
    // The messages for different track systems do not have a common 
    // set of methods/properties. Cast them to its own subtype to access the methods.
    //
    public interface ITrackMessage
    {
        XDocument ToXml();
    }
}