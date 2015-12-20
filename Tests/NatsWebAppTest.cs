using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Nats;
using System.IO;

namespace Tests
{

    [TestClass()]
	public class NatsWebAppTest
	{
		[TestMethod()]
		public void LoadNatsTxtTest()
		{
			string str = File.ReadAllText("Westbound.xml");

			NATsMessage n = new NATsMessage(str);

			Debug.WriteLine(n.Direction);
			Debug.WriteLine(n.GeneralInfo);
			Debug.WriteLine(n.LastUpdated);
			Debug.WriteLine(n.Message);
		}

	}
}
