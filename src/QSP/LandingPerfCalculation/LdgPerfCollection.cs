using QSP.LandingPerfCalculation.Boeing;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSP.LandingPerfCalculation
{
    public static class LdgPerfCollection
    {
        /// <summary>
        /// Load all xml in the landing performance data folder.
        /// </summary>
        public static TableImportResult Initialize()
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

            return new TableImportResult(result.Distinct().ToList(),
                                         message(result));
        }

        private static string message(List<PerfTable> item)
        {
            var groups = item.GroupBy(x => x.Entry.ProfileName);

            try
            {
                var duplicate = groups.First(g => g.Count() > 1);

                return
                    "The following aircrafts have" +
                    " identical profile names:\n" +
                    string.Join("\n", duplicate.Select(x => x.Entry.FilePath));
            }
            catch (InvalidOperationException)
            {
                // There is not duplicate.
                return null;
            }
        }

        public class TableImportResult
        {
            public List<PerfTable> Tables { get; private set; }
            public string Message { get; private set; }

            public TableImportResult(List<PerfTable> Tables, string Message)
            {
                this.Tables = Tables;
                this.Message = Message;
            }
        }
    }
}
