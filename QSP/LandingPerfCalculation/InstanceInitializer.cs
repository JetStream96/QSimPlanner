using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LandingPerfCalculation
{
    public static class InstanceInitializer
    {      
        /// <summary>
        /// Load all xml in the landing performance data folder.
        /// </summary>
        public static List<PerfTable> Initialize()
        {
            var result = new List<PerfTable>();

            foreach (var i in Directory.GetFiles(Constants.Path))
            {
                result.Add(new DataLoader().ReadFromXml(i));
            }

            return result;
        }
    }
}
