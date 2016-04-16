using QSP.LandingPerfCalculation.Boeing;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

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
                try
                {
                    result.Add(new PerfDataLoader().ReadFromXml(i));
                }
                catch (Exception ex)
                {
                    ErrorLogger.WriteToLog(ex);
                }
            }

            return result;
        }
    }
}
