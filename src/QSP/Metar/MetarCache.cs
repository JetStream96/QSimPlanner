using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.Metar
{
    public class MetarCache
    {
        public TimeSpan ExpireTime { get; private set; }
        private Dictionary<string, MetarCacheItem> items = new Dictionary<string, MetarCacheItem>();

        public MetarCache() : this(new TimeSpan(0, 10, 0)) { }

        public MetarCache(TimeSpan expireTime)
        {
            this.ExpireTime = expireTime;
        }

        // Returns null if the item does not exist.
        public MetarCacheItem GetItem(string icao)
        {
            if (items.TryGetValue(icao, out var val))
            {
                if (IsStillValid(val))
                {
                    return val;
                }

                items.Remove(icao);
            }

            return null;
        }

        private bool IsStillValid(MetarCacheItem item)
        {
            return item.CreationTime + ExpireTime <= DateTime.UtcNow;
        }

        public void AddItem(string icao, MetarCacheItem item)
        {
            items.Add(icao, item);
        }
    }
}
