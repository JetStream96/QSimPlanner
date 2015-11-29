namespace QSP.TakeOffPerfCalculation
{

    public class TOPerfParameters
	{

		public int RwyLengthMeter;
		public int RwyElevationFt;
		public int RwyHeading;
		public double RwySlope;
		public int WindHeading;
		public int WindSpeed;
		public int OatCelsius;
		public int QNH;
		public bool SurfaceWet;
		public int TOWT_KG;
		public ThrustRatingOption ThrustRating;
		public AntiIceOption AntiIce;

		public bool PacksOn;
		public TOPerfParameters(int rwyLengthMeter, int rwyElevationFt, int rwyHeading, double rwySlope, int windHeading, int windSpeed, int oatCelsius, int QNH, bool surfaceWet, int takeoffWeightKG,

		ThrustRatingOption thrustRating, AntiIceOption antiIce, bool packsOn)
		{
			this.RwyLengthMeter = rwyLengthMeter;
			this.RwyElevationFt = rwyElevationFt;
			this.RwyHeading = rwyHeading;
			this.RwySlope = rwySlope;
			this.WindHeading = windHeading;
			this.WindSpeed = windSpeed;
			this.OatCelsius = oatCelsius;
			this.QNH = QNH;
			this.SurfaceWet = surfaceWet;
			this.TOWT_KG = takeoffWeightKG;
			this.ThrustRating = thrustRating;
			this.AntiIce = antiIce;
			this.PacksOn = packsOn;

		}

	}

}
