using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Data
{
    /// <summary>
    /// Represents any data that has latitude and longitude property.
    /// </summary>
    public interface ICoordinate
    {
        double Lat { get; }
        double Lon { get; }
    }
}
