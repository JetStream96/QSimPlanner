using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrackMessageNew
    {
        // The messages for different track systems do not have a common 
        // set of methods/properties. So this is a workaround in order to
        // use generics for the entire track subsystem.
        object Property(int i);

        XDocument ToXml();
    }
}