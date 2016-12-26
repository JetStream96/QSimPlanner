using IntegrationTest.QSP.RouteFinding.TestSetup;
using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Pacots;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTest.QSP.RouteFinding.Tracks.Pacots
{
    [TestFixture]
    public class PacotsHandlerTest
    {
        [Test]
        public void GetAllTracksAndAddToWptListTest()
        {
            // Arrange
            var wptList = WptListFactory.GetWptList(wptIdents);
            AddAirways(wptList);

            var recorder = new StatusRecorder();

            var handler = new TrackHandler<PacificTrack>(
                wptList,
                wptList.GetEditor(),
                recorder,
                GetAirportList(),
                new TrackInUseCollection());

            // Act
            handler.GetAllTracks(new DownloaderStub());
            handler.AddToWaypointList();

            // Assert
            Assert.AreEqual(0, recorder.Records.Count);

            // Verify all tracks are added.
            AssertAllTracks(wptList);

            // Check one westbound track.
            AssertTrackJ(wptList);

            // Check one eastbound track.
            AssertTrack11(wptList);

            // Check connection routes
            AssertDct(wptList, "DANNO", "BOOKE"); // In track 11
            AssertDct(wptList, "BRINY", "ALCOA"); // In track J
        }

        private static void AssertDct(
            WaypointList wptList, string from, string to)
        {
            foreach (var i in wptList.EdgesFrom(wptList.FindById(from)))
            {
                if (wptList[wptList.GetEdge(i).ToNodeIndex].ID == to)
                {
                    return;
                }
            }
            Assert.Fail("{0} is not connected to {1}", from, to);
        }

        private static void AssertTrackJ(WaypointList wptList)
        {
            var edge = wptList.GetEdge(
                GetEdgeIndex("PACOTJ", "ALCOA", wptList));

            // Distance
            Assert.AreEqual(
                    new List<ICoordinate>
                    {
                      wptList[wptList.FindById("ALCOA")],
                      wptList[wptList.FindById("CEPAS")],
                      wptList[wptList.FindById("COBAD")],
                      new LatLon(41,-140),
                      new LatLon(42,-150),
                      new LatLon(40,-160),
                      new LatLon(37,-170),
                      new LatLon(33,180),
                      new LatLon(29,170),
                      new LatLon(27,160),
                      new LatLon(26,150),
                      new LatLon(27,140),
                      wptList[wptList.FindById("BIXAK")]
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

        private static void AssertTrack11(WaypointList wptList)
        {
            var edge = wptList.GetEdge(
                GetEdgeIndex("PACOT11", "SEALS", wptList));

            // Distance
            Assert.AreEqual(
                    new List<ICoordinate>
                    {
                      wptList[wptList.FindById("SEALS")],
                      new LatLon(36,150),
                      new LatLon(37,160),
                      new LatLon(36,170),
                      new LatLon(33,180),
                      new LatLon(29,-170),
                      wptList[wptList.FindById("DANNO")]
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

        private static int GetEdgeIndex(string ID, string firstWpt, WaypointList wptList)
        {
            foreach (var i in wptList.EdgesFrom(wptList.FindById(firstWpt)))
            {
                if (wptList.GetEdge(i).Value.Airway == ID)
                {
                    return i;
                }
            }

            return -1;
        }

        private static void AssertAllTracks(WaypointList wptList)
        {
            var id = new[]
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

            var firstWpt = new[]
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
                AssertTrack("PACOT" + id[i], firstWpt[i], wptList);
            }
        }

        private static void AssertTrack(string ID, string firstWpt, WaypointList wptList)
        {
            // check the track is added
            if (GetEdgeIndex(ID, firstWpt, wptList) < 0)
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

        private static AirportManager GetAirportList()
        {
            var collection = new List<Airport>();

            foreach (var i in airports)
            {
                var airport =
                    new Airport(
                        i,
                        "",
                        0.0,
                        0.0,
                        0,
                        true,
                        0,
                        0,
                        0,
                        new List<RwyData>());

                collection.Add(airport);
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

        private static int TryAddWpt(WaypointList wptList, string id)
        {
            int x = wptList.FindById(id);

            if (x < 0)
            {
                var rd = new Random(123);

                return wptList.AddWaypoint(
                    new Waypoint(id, rd.Next(-90, 91), rd.Next(-180, 181)));
            }
            return x;
        }

        private static void AddAirways(WaypointList wptList)
        {
            foreach (var i in airwayEntries)
            {
                int x = TryAddWpt(wptList, i.StartWpt);
                int y = TryAddWpt(wptList, i.EndWpt);
                var neighbor = new Neighbor(i.Airway, wptList.Distance(x, y));

                wptList.AddNeighbor(x, y, neighbor);
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

        private class DownloaderStub : ITrackMessageProvider
        {
            public ITrackMessage GetMessage()
            {
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                var path = directory +
                           "/QSP/RouteFinding/Tracks/Pacots/Defense Internet NOTAM Service.html";

                return new PacotsMessage(File.ReadAllText(path));
            }

            public Task<ITrackMessage> GetMessageAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
