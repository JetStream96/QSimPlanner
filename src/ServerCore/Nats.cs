using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace ServerCore
{
    public class Nats
    {
        private readonly double RefreshIntervalSec =
#if DEBUG
        30;
#else
        60 * 5;
#endif
        private SharedData shared;

        public Nats(IHostingEnvironment env)
        {
            shared = SharedData.GetInstance(env);
        }

        private static List<IndividualNatsMessage> DownloadNats()
        {
            var wc = new WebClient()
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
            };

            var str = wc.DownloadString("https://www.notams.faa.gov/common/nat.html?");
            return new MessageSplitter(str).Split();
        }

        // @Throws
        private void SaveNats()
        {
            var result = DownloadNats();
            Directory.CreateDirectory(shared.MapPath("nats"));
            bool westUpdated = false;
            bool eastUpdated = false;

            foreach (var i in result)
            {
                if (i.Direction == NatsDirection.East)
                {
                    eastUpdated = SaveEastbound(i);
                    if (eastUpdated) LastUpdateTime.SaveEast(shared);
                }
                else
                {
                    westUpdated = SaveWestbound(i);
                    if (westUpdated) LastUpdateTime.SaveWest(shared);
                }
            }

            shared.Logger.Log(SaveNatsMsg(westUpdated, eastUpdated));
        }

        // Test if the eastbound track needs to be saved. If yes, saved the file and 
        // return true. Otherwise, returns false.
        private bool SaveEastbound(IndividualNatsMessage i)
        {
            var filepath = "~/nats/Eastbound.xml";
            var (success, newTime) = NatsMessage.ParseDate(i.LastUpdated);
            if (success && newTime > LastUpdateTime.EastUtc)
            {
                LastUpdateTime.EastUtc = newTime;
                File.WriteAllText(shared.MapPath(filepath), i.ConvertToXml().ToString());
                return true;
            }

            return false;
        }

        private bool SaveWestbound(IndividualNatsMessage i)
        {
            var filepath = "~/nats/Westbound.xml";
            var (success, newTime) = NatsMessage.ParseDate(i.LastUpdated);
            if (success && newTime > LastUpdateTime.WestUtc)
            {
                LastUpdateTime.WestUtc = newTime;
                File.WriteAllText(shared.MapPath(filepath), i.ConvertToXml().ToString());
                return true;
            }

            return false;
        }

        private string SaveNatsMsg(bool westUpdated, bool eastUpdated)
        {
            if (westUpdated)
            {
                return eastUpdated ? "Both directions updated." : "Westbound updated.";
            }

            return eastUpdated ? "Eastbound updated." : "Neither direction updated.";
        }
    }
}
