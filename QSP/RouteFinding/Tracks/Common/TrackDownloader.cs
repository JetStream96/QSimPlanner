using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackDownloader<T> where T : ITrack
    {
        public abstract TrackRawData<T> Download();
        public abstract Task<TrackRawData<T>> DownloadAsync();
    }
}
