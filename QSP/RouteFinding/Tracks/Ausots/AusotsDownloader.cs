using System;
using System.Net;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public static class AusotsDownloader
	{
		private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

		public static string DownloadTrackMessage()
		{
			return (new WebClient()).DownloadString(address);
		}

		public async static Task<string> DownloadTrackMessageAsync()
		{
			return await(new WebClient()).DownloadStringTaskAsync(new Uri(address));
		}
	}
}
