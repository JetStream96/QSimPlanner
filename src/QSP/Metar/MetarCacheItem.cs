using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.WindAloft;
using QSP.Utilities.Units;

namespace QSP.Metar
{
    public class MetarCacheItem
    {
        public string Metar { get; private set; }
        public Wind? Wind { get; private set; }
        public int? Temp { get; private set; }
        public bool PrecipitationExists { get; private set; }
        public (bool hasValue, PressureUnit unit, double value) Pressure { get; private set; }
        public DateTime CreationTime { get; private set; }

        public MetarCacheItem(
            string metar,
            Wind? wind, 
            int? temp, 
            bool precipitation,
            (bool hasValue, PressureUnit unit, double value) Pressure,
            DateTime creationTime)
        {
            this.Metar = metar;
            this.Wind = wind;
            this.Temp = temp;
            this.PrecipitationExists = precipitation;
            this.Pressure = Pressure;
            this.CreationTime = creationTime;
        }

        public static MetarCacheItem Create(string metar)
        {
            return new MetarCacheItem(
                metar,
                ParaExtractor.GetWind(metar),
                ParaExtractor.GetTemp(metar),
                ParaExtractor.PrecipitationExists(metar),
                ParaExtractor.GetPressure(metar),
                DateTime.UtcNow);
        }
    }
}
