using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using System.IO;
using static Test.Common.Utilities;
 
namespace Test
{

    [TestClass()]
    public class AusotsTest
    {

        //[TestMethod()]
        //public void DownloaderTest()
        //{
        //    File.WriteAllText("ausots.txt", AusotsDownloader.DownloadMsg());
        //}

        [TestMethod()]
        public void AusotsConstructorTest()
        {
            PrepareTest();
            var trk = new AusTrack(@"TDM TRK BY16 151114120001 
            1511141100 1511142200
            EML BOXER RUSSO 18S140E 15S135E DN JULIE CURLY IKUMA KIKEM
            RTS / YBBN BN V179 IBUNA Q473 HAWKE V129 EML
            RMK / AUSOTS GROUP A");

        }

        [TestMethod()]
        public void AusotsReaderTest()
        {
            PrepareTest();
            AusTrack trk = new AusTrack(@"TDM TRK BY16 151114120001 
			1511141100 1511142200 
			EML BOXER RUSSO 18S140E 15S135E DN JULIE CURLY IKUMA KIKEM 
			RTS/YBBN BN V179 IBUNA Q473 HAWKE V129 EML 
			RMK/AUSOTS GROUP A");

            TrackReader reader = new TrackReader(trk);

        }

    }
}
