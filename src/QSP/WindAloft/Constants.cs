using System.Collections.Generic;
using static QSP.LibraryExtension.Types;

namespace QSP.WindAloft
{
    public static class Constants
    {
        public static readonly IReadOnlyList<int> FullWindDataSet =
           List(100, 200, 250, 300, 350, 400, 500, 600, 700, 850);

        public static readonly string WxFileDownloadDirectory = @"Wx\tmp";
    }
}
