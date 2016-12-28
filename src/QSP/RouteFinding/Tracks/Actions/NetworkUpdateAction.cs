using System;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Actions
{
    public class NetworkUpdateAction
    {
        public Action<TrackType> SyncTrackEnabled { get; }
        public Action<TrackType> DownloadAndEnable { get; }

        public NetworkUpdateAction(Action<TrackType> SyncTrackEnabled,
            Action<TrackType> DownloadAndEnable)
        {
            this.SyncTrackEnabled = SyncTrackEnabled;
            this.DownloadAndEnable = DownloadAndEnable;
        }
    }
}