using System;
using QSP.Common;

namespace QSP.WindAloft
{
    public class DescendForcastGenerator
	{
		private double lat;
		private double lon;

		private int[] FLs;
		public DescendForcastGenerator(double latitude, double longitude, int[] flightLevels)
		{
			lat = latitude;
			lon = longitude;
			FLs = flightLevels;
		}

		public Wind[] Generate()
		{
			Wind[] forcast = new Wind[FLs.Length];

			for (int i = 0; i < FLs.Length; i++) {
				Tuple<double, double> UVWind = QspCore.WxReader.GetWindUV(lat, lon, FLs[i]);
				Wind w = new Wind();
				w.SetUV(UVWind.Item1, UVWind.Item2);

				forcast[i] = w;
			}
			return forcast;
		}

	}
}
