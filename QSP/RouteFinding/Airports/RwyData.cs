namespace QSP.RouteFinding.Airports
{
    public class RwyData
    {
        public string RwyIdent { get; set; }
        public string Heading { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public bool IlsAvail { get; set; }
        public string IlsFreq { get; set; }
        public string IlsHeading { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int Elevation { get; set; }
        public double GlideslopeAngle { get; set; }
        public int ThresholdOverflyHeight { get; set; }
        public int SurfaceType { get; set; }
        public int RwyStatus { get; set; }

        public RwyData(string RwyIdent, string Heading, int Length, int Width, bool IlsAvail, string IlsFreq, 
                       string IlsHeading, double Lat, double Lon, int Elevation,
                       double GlideslopeAngle, int ThresholdOverflyHeight, int SurfaceType, int RwyStatus)
        {
            this.RwyIdent = RwyIdent;
            this.Heading = Heading;
            this.Length = Length;
            this.Width = Width;
            this.IlsAvail = IlsAvail;
            this.IlsFreq = IlsFreq;
            this.IlsHeading = IlsHeading;
            this.Lat = Lat;
            this.Lon = Lon;
            this.Elevation = Elevation;
            this.GlideslopeAngle = GlideslopeAngle;
            this.ThresholdOverflyHeight = ThresholdOverflyHeight;
            this.SurfaceType = SurfaceType;
            this.RwyStatus = RwyStatus;
        }

        public RwyData(RwyData item)
        {
            RwyIdent = item.RwyIdent;
            Heading = item.Heading;
            Length = item.Length;
            Width = item.Width;
            IlsAvail = item.IlsAvail;
            IlsFreq = item.IlsFreq;
            IlsHeading = item.IlsHeading;
            Lat = item.Lat;
            Lon = item.Lon;
            Elevation = item.Elevation;
            GlideslopeAngle = item.GlideslopeAngle;
            ThresholdOverflyHeight = item.ThresholdOverflyHeight;
            SurfaceType = item.SurfaceType;
            RwyStatus = item.RwyStatus;
        }
    }
}
