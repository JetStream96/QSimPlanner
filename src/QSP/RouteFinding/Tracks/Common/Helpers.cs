using System;
using System.Collections.Generic;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;

namespace QSP.RouteFinding.Tracks.Common
{
    public static class Helpers
    {
        public static ITrackMessageProvider GetTrackDownloader<T>() where T : Track
        {
            return GetTrackDownloader(GetTrackType<T>());
        }

        public static ITrackMessageProvider GetTrackDownloader(TrackType t)
        {
            // TODO: Add more.
            return new[] { new NatsDownloader() }[(int)t];
        }

        public static ITrackParser<T> GetParser<T>(ITrackMessageNew msg, 
            StatusRecorder statusRecorder, AirportManager airportList) where T : Track
        {
            // TODO: Add more.
            return (ITrackParser<T>)new[]
            {
                new NatsParser(msg, statusRecorder, airportList)
            }[(int) GetTrackType<T>()];
        }

        public static TrackType GetTrackType<T>() where T : Track
        {
            return new Dictionary<Type, TrackType>()
            {
                [typeof(NorthAtlanticTrack)] = TrackType.Nats,
                [typeof(PacificTrack)] = TrackType.Pacots,
                [typeof(AusTrack)] = TrackType.Ausots
            }[typeof(T)];
        }

        public static string TrackString(this TrackType item)
        {
            return new[] { "NATs", "PACOTs", "AUSOTS" }[(int)item];
        }
    }
}