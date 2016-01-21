namespace QSP.LandingPerfCalculation
{
    public enum SurfaceCondition
	{
		Dry = 0,
		Good = 1,
		Medium = 2,
		Poor = 3
	}

	public enum ReverserOption
	{
		Both = 0,
		HalfRev = 1,		//one reverser for twin-engined aircraft, two reversers for four-engined aircraft
		NoRev = 2
	}
}
