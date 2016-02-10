using IntegrationTest.QSP.RouteFinding.TestSetup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Routes.Toggler;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Nats.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static QSP.MathTools.Utilities;

namespace IntegrationTest.QSP.RouteFinding.Tracks.Nats
{
    [TestClass]
    public class NatsHandlerTest
    {
        [TestMethod]
        public void GetAllTracksAndAddToWptListTest()
        {
            // Arrange
            var wptList = WptListFactory.GetWptList(wptIdents());
            var recorder = new StatusRecorder();

            var handler = new NatsHandler(new downloaderStub(),
                                        wptList,
                                        wptList.GetEditor(),
                                        recorder,
                                        new AirportManager(new AirportCollection()),
                                        new RouteTrackCommunicator(new TrackInUseCollection()));
            // Act
            handler.GetAllTracks();
            handler.AddToWaypointList();

            // Assert
            Assert.AreEqual(0, recorder.Records.Count);

            // Verify all tracks are added.
            assertAllTracks(wptList);

            // Check one westbound track.
            assertTrackC(wptList);

            // Check one eastbound track.
            assertTrackZ(wptList);
        }

        private static void assertTrackC(WaypointList wptList)
        {
            var edge = wptList.GetEdge(getEdgeIndex("NATC", "ETARI", wptList));

            // Distance
            Assert.AreEqual(
                getDistance(
                    new List<LatLon>
                    {
                      wptList[ wptList.FindByID("ETARI")].LatLon,
                      new LatLon(55.5,-20),
                      new LatLon(55.5,-30),
                      new LatLon(55.5,-40),
                      new LatLon(54.5,-50),
                      wptList[wptList.FindByID("MELDI")].LatLon
                    }),
                edge.Value.Distance,
                0.01);

            // Start, end waypoints are correct
            Assert.IsTrue(wptList[edge.FromNodeIndex].ID == "ETARI");
            Assert.IsTrue(wptList[edge.ToNodeIndex].ID == "MELDI");

            // Start, end waypoints are connected
            Assert.IsTrue(wptList.EdgesFromCount(edge.FromNodeIndex) > 0);
            Assert.IsTrue(wptList.EdgesToCount(edge.ToNodeIndex) > 0);

            // Airway is correct
            Assert.IsTrue(edge.Value.Airway == "NATC");
        }

        private static void assertTrackZ(WaypointList wptList)
        {
            var edge = wptList.GetEdge(getEdgeIndex("NATZ", "SOORY", wptList));

            // Distance
            Assert.AreEqual(
                getDistance(
                    new List<LatLon>
                    {
                      wptList[ wptList.FindByID("SOORY")].LatLon,
                      new LatLon(42.0,-50.0),
                      new LatLon(44.0,-40.0),
                      new LatLon(44.0,-30.0),
                      new LatLon(46.0,-20.0),
                      new LatLon(46.0,-15.0),
                      wptList[wptList.FindByID("SEPAL")].LatLon,
                      wptList[wptList.FindByID("LAPEX")].LatLon
                    }),
                edge.Value.Distance,
                0.01);

            // Start, end waypoints are correct
            Assert.IsTrue(wptList[edge.FromNodeIndex].ID == "SOORY");
            Assert.IsTrue(wptList[edge.ToNodeIndex].ID == "LAPEX");

            // Start, end waypoints are connected
            Assert.IsTrue(wptList.EdgesFromCount(edge.FromNodeIndex) > 0);
            Assert.IsTrue(wptList.EdgesToCount(edge.ToNodeIndex) > 0);

            // Airway is correct
            Assert.IsTrue(edge.Value.Airway == "NATZ");
        }

        private static double getDistance(List<LatLon> item)
        {
            if (item.Count < 2)
            {
                return 0.0;
            }

            double d = 0.0;

            for (int i = 0; i < item.Count - 1; i++)
            {
                d += GreatCircleDistance(item[i], item[i + 1]);
            }
            return d;
        }

        private static int getEdgeIndex(string ID, string firstWpt, WaypointList wptList)
        {
            foreach (var i in wptList.EdgesFrom(wptList.FindByID(firstWpt)))
            {
                if (wptList.GetEdge(i).Value.Airway == ID)
                {
                    return i;
                }
            }
            return -1;
        }

        private static void assertAllTracks(WaypointList wptList)
        {
            var id = new char[] { 'A', 'B', 'C', 'D', 'E', 'F',
                'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            var firstWpt = new string[]
            { "SUNOT",
              "PIKIL",
              "ETARI",
              "RESNO",
              "DOGAL",
              "MALOT",
              "NICSO",
              "PORTI",
              "SUPRY",
              "RAFIN",
              "DOVEY",
              "JOBOC",
              "SLATN",
              "SOORY"
            };

            for (int i = 0; i < id.Length; i++)
            {
                assertTrack("NAT" + id[i], firstWpt[i], wptList);
            }
        }

        private static void assertTrack(string ID, string firstWpt, WaypointList wptList)
        {
            // check the track is added
            if (getEdgeIndex(ID, firstWpt, wptList) < 0)
            {
                Assert.Fail("Track not found.");
            }
        }

        private static List<string> wptIdents()
        {
            return new List<string>
            { "SUNOT",
              "JANJO",
              "PIKIL",
              "LOMSI",
              "ETARI",
              "MELDI",
              "RESNO",
              "NEEKO",
              "DOGAL",
              "RIKAL",
              "MALOT",
              "TUDEP",
              "NICSO",
              "LIMRI",
              "XETBO",
              "PORTI",
              "DINIM",
              "ELSOX",
              "SUPRY",
              "SOMAX",
              "ATSUR",
              "RAFIN",
              "BEDRA",
              "NERTU",
              "DOVEY",
              "OMOKO",
              "GUNSO",
              "JOBOC",
              "ETIKI",
              "REGHI",
              "SLATN",
              "SOORY",
              "SEPAL",
              "LAPEX"
            };
        }

        private class downloaderStub : INatsDownloader
        {
            public NatsMessage Download()
            {
                var htmlSource = File.ReadAllText("QSP/RouteFinding/Tracks/Nats/North Atlantic Tracks.html");
                var msgs = new MessageSplitter(htmlSource).Split();
                int westIndex = msgs[0].Direction == NatsDirection.West ? 0 : 1;
                int eastIndex = 1 - westIndex;
                return new NatsMessage(msgs[westIndex], msgs[eastIndex]);
            }

            public Task<NatsMessage> DownloadAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
