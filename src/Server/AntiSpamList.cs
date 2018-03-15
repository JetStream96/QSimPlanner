using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TaskUtil = QSP.LibraryExtension.Tasks.Util;

namespace Server
{
    public class AntiSpamList
    {
        public const int DefaultTokenCount = 120;
        public const int TokensPerHour = 120;

        // Maps ip to token count.
        private Dictionary<string, int> users = new Dictionary<string, int>();

        public AntiSpamList() { }

        // @NoThrow
        public async void Start()
        {
            await TaskUtil.RunPeriodic(IncrementToken,
                new TimeSpan(0, 0, 60 * 60 / TokensPerHour), new CancellationToken());
        }

        // @NoThrow
        private void IncrementToken()
        {
            // This code is complex because we cannot change dictionary while iterating.

            var u = users;
            var keys = u.Keys.ToList();

            // Increment
            keys.ForEach(ip => u[ip]++);

            // Remove IPs exceeding DefaultTokenCount.
            var removeList = keys.Where(ip => u[ip] >= DefaultTokenCount).ToList();
            removeList.ForEach(ip => u.Remove(ip));
        }

        // Returns whether the ip is spammer.
        public bool DecrementToken(string ip)
        {
            var u = users;

            if (!u.TryGetValue(ip, out var count))
            {
                u[ip] = DefaultTokenCount;
                count = DefaultTokenCount;
            }

            if (count == 0) return true;

            u[ip]--;
            return false;
        }

        // @NoThrow
        public bool IsSpammer(string ip)
        {
            if (!users.TryGetValue(ip, out var token)) return false;
            return token == 0;
        }
    }
}