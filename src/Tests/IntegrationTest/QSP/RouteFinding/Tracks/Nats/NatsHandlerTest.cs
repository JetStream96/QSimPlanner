using IntegrationTest.QSP.RouteFinding.TestSetup;
using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Nats.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using QSP.RouteFinding.Data.Interfaces;

namespace IntegrationTest.QSP.RouteFinding.Tracks.Nats
{
    [TestFixture]
    public class NatsHandlerTest
    {
        [Test]
        public void GetAllTracksAndAddToWptListTest()
        {
            // Arrange
            var wptList = WptListFactory.GetWptList(WptIdents());
            var recorder = new StatusRecorder();

            var handler = new NatsHandler(
                wptList,
                wptList.GetEditor(),
                recorder,
                new AirportManager(),
                new TrackInUseCollection());

            // Act
            handler.GetAllTracks(new DownloaderStub());
            handler.AddToWaypointList();

            // Assert
            Assert.AreEqual(0, recorder.Records.Count);

            // Verify all tracks are added.
            AssertAllTracks(wptList);

            // Check one westbound track.
            AssertTrackC(wptList);

            // Check one eastbound track.
            AssertTrackZ(wptList);
        }

        private static void AssertTrackC(WaypointList wptList)
        {
            var edge = wptList.GetEdge(GetEdgeIndex("NATC", "ETARI", wptList));

            // Distance
            Assert.AreEqual(                
                    new List<ICoordinate>
                    {
                      wptList[wptList.FindById("ETARI")],
                      new LatLon(55.5,-20),
                      new LatLon(55.5,-30),
                      new LatLon(55.5,-40),
                      new LatLon(54.5,-50),
                      wptList[wptList.FindById("MELDI")]
                    }.TotalDistance(),
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

        private static void AssertTrackZ(WaypointList wptList)
        {
            var edge = wptList.GetEdge(GetEdgeIndex("NATZ", "SOORY", wptList));

            // Distance
            Assert.AreEqual(
                new List<ICoordinate>
                {
                    wptList[wptList.FindById("SOORY")],
                    new LatLon(42.0,-50.0),
                    new LatLon(44.0,-40.0),
                    new LatLon(44.0,-30.0),
                    new LatLon(46.0,-20.0),
                    new LatLon(46.0,-15.0),
                    wptList[wptList.FindById("SEPAL")],
                    wptList[wptList.FindById("LAPEX")]
                }.TotalDistance(),
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
        
        private static int GetEdgeIndex(
            string ID, string firstWpt, WaypointList wptList)
        {
            foreach (var i in wptList.EdgesFrom(wptList.FindById(firstWpt)))
            {
                if (wptList.GetEdge(i).Value.Airway == ID) return i;
            }

            return -1;
        }

        private static void AssertAllTracks(WaypointList wptList)
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
                AssertTrack("NAT" + id[i], firstWpt[i], wptList);
            }
        }

        private static void AssertTrack(
            string ID, string firstWpt, WaypointList wptList)
        {
            // check the track is added
            if (GetEdgeIndex(ID, firstWpt, wptList) < 0)
            {
                Assert.Fail("Track not found.");
            }
        }

        private static List<string> WptIdents()
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

        private class DownloaderStub : INatsMessageProvider
        {
            public NatsMessage GetMessage()
            {
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                var htmlSource = File.ReadAllText(
                    directory +
                    "/QSP/RouteFinding/Tracks/Nats/North Atlantic Tracks.html");
                var msgs = new MessageSplitter(htmlSource).Split();

                int westIndex =
                    msgs[0].Direction == NatsDirection.West ? 0 : 1;
                int eastIndex = 1 - westIndex;
                return new NatsMessage(msgs[westIndex], msgs[eastIndex]);
            }
        }
    }
}
