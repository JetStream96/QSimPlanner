using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Common
{
   public interface IXmlConvertible
    {
        XDocument ToXml();
        void LoadFromXml(XDocument doc);        
    }
}
