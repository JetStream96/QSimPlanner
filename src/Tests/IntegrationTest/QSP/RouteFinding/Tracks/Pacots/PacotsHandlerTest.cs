using IntegrationTest.QSP.RouteFinding.TestSetup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes.Toggler;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using QSP.RouteFinding.Data.Interfaces;

namespace IntegrationTest.QSP.RouteFinding.Tracks.Pacots
{
    [TestClass]
    public class PacotsHandlerTest
    {
        [TestMethod]
        public void GetAllTracksAndAddToWptListTest()
        {
            // Arrange
            var wptList = WptListFactory.GetWptList(wptIdents);
            addAirways(wptList);

            var recorder = new StatusRecorder();

            var handler = new PacotsHandler(new downloaderStub(),
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

            // Check one westbound track.
            assertTrackJ(wptList);

            // Check one eastbound track.
            assertTrack11(wptList);

            // Check connection routes
            assertDct(wptList, "DANNO", "BOOKE"); // In track 11
            assertDct(wptList, "BRINY", "ALCOA"); // In track J
        }

        private static void assertDct(WaypointList wptList, string from, string to)
        {
            foreach (var i in wptList.EdgesFrom(wptList.FindByID(from)))
            {
                if (wptList[wptList.GetEdge(i).ToNodeIndex].ID == to)
                {
                    return;
                }
            }
            Assert.Fail("{0} is not connected to {1}", from, to);
        }

        private static void assertTrackJ(WaypointList wptList)
        {
            var edge = wptList.GetEdge(getEdgeIndex("PACOTJ", "ALCOA", wptList));

            // Distance
            Assert.AreEqual(
                    new List<LatLon>
                    {
                      wptList[ wptList.FindByID("ALCOA")].LatLon,
                      wptList[ wptList.FindByID("CEPAS")].LatLon,
                      wptList[ wptList.FindByID("COBAD")].LatLon,
                      new LatLon(41,-140),
                      new LatLon(42,-150),
                      new LatLon(40,-160),
                      new LatLon(37,-170),
                      new LatLon(33,180),
                      new LatLon(29,170),
                      new LatLon(27,160),
                      new LatLon(26,150),
                      new LatLon(27,140),
                      wptList[ wptList.FindByID("BIXAK")].LatLon
                    }.TotalDistance(),
                edge.Value.Distance,
                0.01);

            // Start, end waypoints are correct
            Assert.IsTrue(wptList[edge.FromNodeIndex].ID == "ALCOA");
            Assert.IsTrue(wptList[edge.ToNodeIndex].ID == "BIXAK");

            // Start, end waypoints are connected
            Assert.IsTrue(wptList.EdgesFromCount(edge.FromNodeIndex) > 0);
            Assert.IsTrue(wptList.EdgesToCount(edge.ToNodeIndex) > 0);

            // Airway is correct
            Assert.IsTrue(edge.Value.Airway == "PACOTJ");
        }

        private static void assertTrack11(WaypointList wptList)
        {
            var edge = wptList.GetEdge(getEdgeIndex("PACOT11", "SEALS", wptList));

            // Distance
            Assert.AreEqual(
                    new List<LatLon>
                    {
                      wptList[ wptList.FindByID("SEALS")].LatLon,
                      new LatLon(36,150),
                      new LatLon(37,160),
                      new LatLon(36,170),
                      new LatLon(33,180),
                      new LatLon(29,-170),
                      wptList[wptList.FindByID("DANNO")].LatLon
                    }.TotalDistance(),
                edge.Value.Distance,
                0.01);

            // Start, end waypoints are correct
            Assert.IsTrue(wptList[edge.FromNodeIndex].ID == "SEALS");
            Assert.IsTrue(wptList[edge.ToNodeIndex].ID == "DANNO");

            // Start, end waypoints are connected
            Assert.IsTrue(wptList.EdgesFromCount(edge.FromNodeIndex) > 0);
            Assert.IsTrue(wptList.EdgesToCount(edge.ToNodeIndex) > 0);

            // Airway is correct
            Assert.IsTrue(edge.Value.Airway == "PACOT11");
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
                "8",
                "J",
                "K",
                "A",
                "C",
                "E",
                "F",
                "M",
                "H",
                "1",
                "2",
                "3",
                "11",
                "14"
            };

            var firstWpt = new string[]
            {
                "KALNA",
                "ALCOA",
                "DINTY",
                "LILIA",
                "JOWEN",
                "LINUZ",
                "ALCOA",
                "KATCH",
                "ALCOA",
                "KALNA",
                "EMRON",
                "LEPKI",
                "SEALS",
                "LEPKI"
            };

            for (int i = 0; i < id.Length; i++)
            {
                assertTrack("PACOT" + id[i], firstWpt[i], wptList);
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
            "ADNAP",
            "ALCOA",
            "ALLBE",
            "AMAKR",
            "ARCAL",
            "AVBET",
            "AVE",
            "BGGLO",
            "BIXAK",
            "BOARS",
            "BOOKE",
            "BRINY",
            "BUNGU",
            "CANAI",
            "CEPAS",
            "CJAYY",
            "COBAD",
            "CREMR",
            "CUNDU",
            "DACEM",
            "DANNO",
            "DELTA",
            "DINTY",
            "DOGIF",
            "EMRON",
            "ENI",
            "FERAR",
            "FIM",
            "FINGS",
            "FOCHE",
            "FORDO",
            "GNNRR",
            "GRIZZ",
            "GUMBO",
            "HMPTN",
            "IGURU",
            "JOWEN",
            "KAGIS",
            "KALNA",
            "KATCH",
            "KEIKO",
            "KEOLA",
            "LEPKI",
            "LIBBO",
            "LILIA",
            "LINUZ",
            "LOHNE",
            "MARNR",
            "MOLKA",
            "MORAY",
            "NANAC",
            "NANDY",
            "NATES",
            "NATTE",
            "NHC",
            "NIKLL",
            "NIPPI",
            "NUZAN",
            "NYMPH",
            "OATIS",
            "OBOYD",
            "OGDEN",
            "OGGOE",
            "OLCOT",
            "OMOTO",
            "ONC",
            "ONEIL",
            "ONION",
            "OPAKE",
            "OPHET",
            "OSI",
            "PAINT",
            "PINTT",
            "PIRAT",
            "PRETY",
            "QQ",
            "RZS",
            "SEALS",
            "SEDKU",
            "SEFIX",
            "SELDM",
            "SMOLT",
            "STINS",
            "SYOYU",
            "TAMRU",
            "TOU",
            "TRYSH",
            "TUNTO",
            "VACKY",
            "YAZ",
            "YZT",
            "ZANNG"
        }.Distinct().ToList();

        private static List<string> airports = new List<string>
        {
            "KLAX",
            "KDFW",
            "KSFO",
            "PHNL",
            "CYVR",
            "KSEA",
            "KPDX"
        };

        private static AirportManager getAirportList()
        {
            var collection = new AirportCollection();

            foreach (var i in airports)
            {
                collection.Add(new Airport(i, "", 0.0, 0.0, 0,true, 0, 0, 0, new List<RwyData>()));
            }

            return new AirportManager(collection);
        }

        private static List<airwayEntry> airwayEntries = new List<airwayEntry>
        {
            new airwayEntry("TUNTO","R595","SEDKU"),
            new airwayEntry("OMOTO","R580","OATIS"),
            new airwayEntry("MORAY","OTR15","SMOLT"),
            new airwayEntry("NIPPI","R220","NANAC"),
            new airwayEntry("CANAI","V75","NHC"),
            new airwayEntry("ONION","OTR5","KALNA"),
            
        //  new airwayEntry("ONION","OTR5","ADNAP"),
            new airwayEntry("KALNA","OTR5","ADNAP"),

            new airwayEntry("ADNAP","OTR7","EMRON"),
            new airwayEntry("AVBET","OTR11","LEPKI"),
            new airwayEntry("VACKY","OTR13","SEALS"),
            new airwayEntry("MOLKA", "M750", "BUNGU"),
            new airwayEntry("BUNGU", "Y81", "SYOYU"),
            new airwayEntry("SYOYU", "Y809", "KAGIS"),
            new airwayEntry("KAGIS", "OTR11", "LEPKI")
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

        private class downloaderStub : IPacotsDownloader
        {
            public PacotsMessage Download()
            {
                return new PacotsMessage(
                    File.ReadAllText(
                        "QSP/RouteFinding/Tracks/Pacots/Defense Internet NOTAM Service.html"));
            }

            public Task<PacotsMessage> DownloadAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
