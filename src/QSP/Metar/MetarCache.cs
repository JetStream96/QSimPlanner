using System;
using System.Collections.Generic;
using CommonLibrary.AviationTools;

namespace QSP.Metar
{
    // Case-insensitive for all ICAO codes.
    public class MetarCache
    {
        public TimeSpan ExpireTime { get; private set; }
        private Dictionary<string, MetarCacheItem> items = new Dictionary<string, MetarCacheItem>();

        public MetarCache() : this(new TimeSpan(0, 10, 0)) { }

        public MetarCache(TimeSpan expireTime)
        {
            this.ExpireTime = expireTime;
        }

        // @NoThrow
        public bool Contains(string icao) => GetItem(icao) != null;

        // @NoThrow
        // Returns null if the item already expired or does not exist.
        public MetarCacheItem GetItem(string icao)
        {
            var trimed = Icao.TrimIcao(icao);

            if (items.TryGetValue(trimed, out var val))
            {
                if (IsStillValid(val))
                {
                    return val;
                }

                items.Remove(trimed);
            }

            return null;
        }
        
        private bool IsStillValid(MetarCacheItem item)
        {
            return item.CreationTime + ExpireTime <= DateTime.UtcNow;
        }

        // @Throws
        // Throws exception if item is null.
        public void AddOrUpdateItem(string icao, MetarCacheItem item)
        {
            // This is important for the correctness for Contains(icao) method. 
            if (item == null) throw new ArgumentException();

            var trimed = Icao.TrimIcao(icao);

            if (items.ContainsKey(trimed))
            {
                items[trimed] = item;
            }
            else
            {
                items.Add(trimed, item);
            }
        }
    }
}
