using System;
using QSP.WindAloft;
using QSP.Core.Options;

namespace QSP.Core
{
    public static class QspCore
	{
		public static PerformanceData PerfDB = new PerformanceData();
		
		public static WxFileLoader WxReader;
		public static string QspLocalDirectory = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData) + "\\QSimPlanner";

		public static string QspAppDataDirectory = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData) + "\\QSimPlanner";
	}
}
