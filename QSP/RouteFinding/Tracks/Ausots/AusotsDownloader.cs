using System;
using System.Net;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{

    public static class AusotsDownloader
	{


		private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";
		public static string DownloadMsg()
		{
			return (new WebClient()).DownloadString(address);
		}

		public async static Task<string> DownloadMsgAsync()
		{
			return await(new WebClient()).DownloadStringTaskAsync(new Uri(address));
		}

	}

}
