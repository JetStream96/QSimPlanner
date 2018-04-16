using QSP.RouteFinding.Data.Interfaces;
using System;

namespace QSP.RouteFinding.Navaids
{
    /// <summary>
    /// Note IsVOR and IsDME can be both true (VOR/DME).
    /// </summary>
    public class Navaid : ICoordinate, IEquatable<Navaid>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Freq { get; set; }
        public bool IsVOR { get; set; }
        public bool IsDME { get; set; }
        public bool IsNDB => !IsVOR && !IsDME;
        public int RangeNm { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int ElevationFt { get; set; }

        /// <summary>
        /// ISO country code. 
        /// This country code may have nothing to do with the others in this program.
        /// </summary>
        public string CountryCode { get; set; }

        public bool Equals(Navaid other)
        {
            return other != null &&
                   ID == other.ID &&
                   Name == other.Name &&
                   Freq == other.Freq &&
                   IsVOR == other.IsVOR &&
                   IsDME == other.IsDME &&
                   RangeNm == other.RangeNm &&
                   Lat == other.Lat &&
                   Lon == other.Lon &&
                   ElevationFt == other.ElevationFt &&
                   CountryCode == other.CountryCode;
        }
    }
}
