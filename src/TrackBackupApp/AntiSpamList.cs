using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QSP.LibraryExtension;
using System.Threading;
using QSP.LibraryExtension.Tasks;
using TaskUtil = QSP.LibraryExtension.Tasks.Util;
using System.Threading.Tasks;

namespace TrackBackupApp
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
            var u = users;
            u.Keys.ForEach(ip =>
            {
                if (u[ip]++ >= DefaultTokenCount) users.Remove(ip);
            });

        }

        // Returns whether the ip is spammer.
        public bool DecrementToken(string ip)
        {
            var u = users;

            if (!u.TryGetValue(ip, out var count)) u[ip] = DefaultTokenCount;

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