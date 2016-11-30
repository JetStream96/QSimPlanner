using System.Collections.Generic;

namespace QSP.WindAloft
{
    public static class Constants
    {
        public static readonly IReadOnlyList<int> FullWindDataSet =
           new int[] { 100, 200, 250, 300, 350, 400, 500, 600, 700, 850 };

        public static readonly string WxFileDownloadDirectory = @"Wx\tmp";
    }
}
