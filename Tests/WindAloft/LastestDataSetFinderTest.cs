using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.WindAloft;
using System.Diagnostics;

namespace Tests.WindAloft
{    
    [TestClass]
    public class LastestDataSetFinderTest
    {
        [TestMethod ]
        public void FindUrlTest()
        {
            //Debug.WriteLine(new LastestDataSetFinder().Find());
        }

        [TestMethod]
        public void GribDownloadTest()
        {
            //new GribDownloader().DownloadGribFile(@"E:\Aviation\Projects\Lab\CSLab\123.grib2");
        }
    }
}
