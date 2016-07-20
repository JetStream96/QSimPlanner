using System.Collections.Generic;

namespace QSP.WindAloft
{
    public static class Constants
    {
        private static int[] _fullWindDataSet =
            { 100, 200, 250, 300, 350, 400, 500, 600, 700, 850 };

        public static IReadOnlyList<int> FullWindDataSet
        {
            get
            {
                return _fullWindDataSet;
            }
        }

        public static readonly string WxFileDirectory = @"Wx";
        public static readonly string WxFileDownloadDirectory = @"Wx\tmp";
    }
}
