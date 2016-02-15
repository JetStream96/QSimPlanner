using IntegrationTest.QSP.RouteFinding.TestSetup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Routes.Toggler;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Ausots.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static QSP.MathTools.Utilities;
using QSP.RouteFinding.Containers;

namespace IntegrationTest.QSP.RouteFinding.Tracks.Nats
{
    [TestClass]
    public class AusotsHandlerTest
    {
        [TestMethod]
        public void GetAllTracksAndAddToWptListTest()
        {
            // Arrange
            var wptList = WptListFactory.GetWptList(wptIdents);
            var recorder = new StatusRecorder();

            var handler = new AusotsHandler(new downloaderStub(),
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
            var id = new string[]
            {
                // Lines commented out are unavailable tracks.

                "MY14",
                "SY14",
             // "SK14",             
             // "SX14",                
                "BP14"
            };

            var firstWpt = new string[]
            { "JAMOR",
              "PKS",
              "TAXEG"
            };

            for (int i = 0; i < id.Length; i++)
            {
                assertTrack("AUSOTS" + id[i], firstWpt[i], wptList);
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

        private static List<string> wptIdents = new List<string>
        {
            "JAMOR",
            "IBABI",
            "LEC",
            "OOD",
            "ARNTU",
            "KEXIM",
            "CIN",
            "ATMAP",
            "PKS",
            "KADUV",
            "TAROR",
            "DANAL",
            "AS",
            "DEENO",
            "SANDY",
            "ATMAP",
            "TAXEG",
            "PASTA",
            "TAROR",
            "WR",
            "ENTRE",
            "MALLY",
            "NSM"
        };

        private static List<string> airports = new List<string>
        {
            "YMML",
            "YSSY",
            "YBBN",
            "YPPH"
        };

        private static List<airwayEntry> airwayEntries = new List<airwayEntry>
        {
            new airwayEntry("ML", "H164", "JAMOR"),
            new airwayEntry("TESAT", "H44", "KAT"),
            new airwayEntry("KAT", "A576", "PKS"),
            new airwayEntry("BN", "H62", "LAV"),
            new airwayEntry("LAV", "Q116", "TAXEG"),
            new airwayEntry("NSM", "Q10", "HAMTN"),
            new airwayEntry("HAMTN", "Q158", "PH")
        };

        private static int tryAddWpt(WaypointList wptList,string id)
        {
            int x = wptList.FindByID(id);

            if (x < 0)
            {
                var rd = new Random();

                return wptList.AddWaypoint(
                    new Waypoint( id, rd.Next(-90, 91), rd.Next(-180, 181)));
            }
            return x;
        }

        private static void addAirways(WaypointList wptList)
        {
            foreach (var i in airwayEntries)
            {
                int x = tryAddWpt(wptList, i.StartWpt);
                int y = tryAddWpt(wptList, i.EndWpt);
                wptList.AddNeighbor(x, y, new Neighbor(i.Airway, wptList.Distance(x, y)));
            }
        }

        private class airwayEntry
        {
            public string StartWpt;
            public string Airway;
            public string EndWpt;

            public airwayEntry(string StartWpt, string Airway, string EndWpt)
            {
                this.StartWpt = StartWpt;
                this.Airway = Airway;
                this.EndWpt = EndWpt;
            }
        }

        private class downloaderStub : IAusotsDownloader
        {
            public AusotsMessage Download()
            {
                return new AusotsMessage(File.ReadAllText("QSP/RouteFinding/Tracks/Ausots/text.asp.html"));
            }

            public Task<AusotsMessage> DownloadAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
