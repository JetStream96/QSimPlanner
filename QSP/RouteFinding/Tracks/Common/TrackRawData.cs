using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackRawData<T> : IXmlConvertible  where T :ITrack
    {
        public abstract void LoadFromXml(XDocument doc);
        public abstract override string ToString();
        public abstract XDocument ToXml();
    }
}
