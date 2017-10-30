namespace QSP.RouteFinding.Airports
{
    public interface IRwyData
    {
        string RwyIdent { get; }
        string Heading { get; }
        int LengthFt { get; }
        int WidthFt { get; }
        bool HasIlsInfo { get; }

        // The following 3 values can be used only if HasIlsInfo is true.
        bool IlsAvail { get; }
        string IlsFreq { get; }
        string IlsHeading { get; }

        double Lat { get; }
        double Lon { get; }
        int Elevation { get; }
        double GlideslopeAngle { get; }
        int ThresholdOverflyHeight { get; }
        string SurfaceType { get; }
        int RwyStatus { get; }
    }
}
