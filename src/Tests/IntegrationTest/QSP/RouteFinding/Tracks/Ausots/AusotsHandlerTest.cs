using IntegrationTest.QSP.RouteFinding.TestSetup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes.Toggler;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static QSP.RouteFinding.Utilities;

namespace IntegrationTest.QSP.RouteFinding.Tracks.Ausots
{
    [TestClass]
    public class AusotsHandlerTest
    {
        [TestMethod]
        public void GetAllTracksAndAddToWptListTest()
        {
            // Arrange
            var wptList = WptListFactory.GetWptList(wptIdents);
            addAirways(wptList);

            var recorder = new StatusRecorder();

            var handler = new AusotsHandler(new downloaderStub(),
                                            wptList,
                                            wptList.GetEditor(),
                                            recorder,
                                            getAirportList(),
                                            new RouteTrackCommunicator(new TrackInUseCollection()));
            // Act
            handler.GetAllTracks();
            handler.AddToWaypointList();

            // Assert
            Assert.AreEqual(0, recorder.Records.Count);

            // Verify all tracks are added.
            assertAllTracks(wptList);

            // Check the tracks.
            assertTrackMY14(wptList);
            assertTrackBP14(wptList);
        }

        private static void assertTrackMY14(WaypointList wptList)
        {
            var edge = wptList.GetEdge(getEdgeIndex("AUSOTMY14", "JAMOR", wptList));

            // Distance
            Assert.AreEqual(
                GetTotalDistance(
                    new List<LatLon>
                    {
                      wptList[ wptList.FindByID("JAMOR")].LatLon,
                      wptList[ wptList.FindByID("IBABI")].LatLon,
                      wptList[ wptList.FindByID("LEC")].LatLon,
                      wptList[ wptList.FindByID("OOD")].LatLon,
                      wptList[ wptList.FindByID("ARNTU")].LatLon,
                      wptList[ wptList.FindByID("KEXIM")].LatLon,
                      wptList[ wptList.FindByID("CIN")].LatLon,
                      wptList[ wptList.FindByID("ATMAP")].LatLon
                    }),
                edge.Value.Distance,
                0.01);

            // Start, end waypoints are correct
            Assert.IsTrue(wptList[edge.FromNodeIndex].ID == "JAMOR");
            Assert.IsTrue(wptList[edge.ToNodeIndex].ID == "ATMAP");

            // Start, end waypoints are connected
            Assert.IsTrue(wptList.EdgesFromCount(edge.FromNodeIndex) > 0);
            Assert.IsTrue(wptList.EdgesToCount(edge.ToNodeIndex) > 0);

            // Airway is correct
            Assert.IsTrue(edge.Value.Airway == "AUSOTMY14");
        }

        private static void assertTrackBP14(WaypointList wptList)
        {
            var edge = wptList.GetEdge(getEdgeIndex("AUSOTBP14", "TAXEG", wptList));

            // Distance
            Assert.AreEqual(
                GetTotalDistance(
                    new List<LatLon>
                    {
                      wptList[ wptList.FindByID("TAXEG")].LatLon,
                      wptList[wptList.FindByID("PASTA")].LatLon,
                      wptList[ wptList.FindByID("TAROR")].LatLon,
                      wptList[ wptList.FindByID("WR")].LatLon,
                      wptList[ wptList.FindByID("ENTRE")].LatLon,
                      wptList[ wptList.FindByID("MALLY")].LatLon,
                      wptList[ wptList.FindByID("NSM")].LatLon
                    }),
                edge.Value.Distance,
                0.01);

            // Start, end waypoints are correct
            Assert.IsTrue(wptList[edge.FromNodeIndex].ID == "TAXEG");
            Assert.IsTrue(wptList[edge.ToNodeIndex].ID == "NSM");

            // Start, end waypoints are connected
            Assert.IsTrue(wptList.EdgesFromCount(edge.FromNodeIndex) > 0);
            Assert.IsTrue(wptList.EdgesToCount(edge.ToNodeIndex) > 0);

            // Airway is correct
            Assert.IsTrue(edge.Value.Airway == "AUSOTBP14");
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
            {
                "JAMOR",
                "PKS",
                "TAXEG"
            };

            for (int i = 0; i < id.Length; i++)
            {
                assertTrack("AUSOT" + id[i], firstWpt[i], wptList);
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
            "WR",
            "ENTRE",
            "MALLY",
            "NSM"
        }.Distinct().ToList();

        private static List<string> airports = new List<string>
        {
            "YMML",
            "YSSY",
            "YBBN",
            "YPPH"
        };

        private static AirportManager getAirportList()
        {
            var collection = new AirportCollection();

            foreach (var i in airports)
            {
                collection.Add(new Airport(i, "", 0.0, 0.0, 0, 0, 0, 0, new List<RwyData>()));
            }

            return new AirportManager(collection);
        }

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

        private static int tryAddWpt(WaypointList wptList, string id)
        {
            int x = wptList.FindByID(id);

            if (x < 0)
            {
                var rd = new Random(123);

                return wptList.AddWaypoint(
                    new Waypoint(id, rd.Next(-90, 91), rd.Next(-180, 181)));
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
