using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSP.FuelCalculation.FuelData
{
    public static class FuelDataLoader
    {
        public const string DefaultFolderPath = @"PerformanceData\FuelCalc";

        /// <summary>
        /// Load all xml in the landing performance data folder.
        /// Files in wrong format are ignored.
        /// Files containing the same profile name are not loaded and
        /// a message will be included in returning value.
        /// </summary>
        public static LoadResult Load(string folderPath = DefaultFolderPath)
        {
            var tables = new List<FuelData>();

            foreach (var i in Directory.GetFiles(folderPath))
            {
                try
                {
                    tables.Add(FuelData.FromFile(i));
                }
                catch (Exception ex)
                {
                    LoggerInstance.WriteToLog(ex);
                }
            }

            var groups = tables.GroupBy(x => x.ProfileName);

            var nonDuplicate = groups
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .ToList();

            var msg = Message(tables);

            return new LoadResult() { Data = nonDuplicate, Message = msg };
        }

        public struct LoadResult
        {
            public List<FuelData> Data; public string Message;
        }

        private static string Message(List<FuelData> item)
        {
            var groups = item.GroupBy(x => x.ProfileName);

            try
            {
                var duplicate = groups.First(g => g.Count() > 1);

                return
                    "The following aircrafts have" +
                    " identical profile names:\n\n" +
                    string.Join("\n", duplicate.Select(x => x.FilePath)) +
                    "\n\nNone of these profiles will be loaded.";
            }
            catch (InvalidOperationException)
            {
                // There is no duplicate.
                return null;
            }
        }
    }
}
