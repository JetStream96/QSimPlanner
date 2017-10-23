using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.WindAloft;

namespace QSP.Metar
{
    public class MetarCacheItem
    {
        public string Metar { get; private set; }
        public Wind? Wind { get; private set; }
        public int? Temp { get; private set; }
        public bool PrecipitationExists { get; private set; }
        public DateTime CreationTime { get; private set; }

        public MetarCacheItem(string metar, Wind? wind, int? temp, 
            bool precipitation, DateTime creationTime)
        {
            this.Metar = metar;
            this.Wind = wind;
            this.Temp = temp;
            this.PrecipitationExists = precipitation;
            this.CreationTime = creationTime;
        }

        public static MetarCacheItem Create(string metar)
        {
            return new MetarCacheItem(
                metar,
                ParaExtractor.GetWind(metar),
                ParaExtractor.GetTemp(metar),
                ParaExtractor.PrecipitationExists(metar),
                DateTime.UtcNow);
        }
    }
}
