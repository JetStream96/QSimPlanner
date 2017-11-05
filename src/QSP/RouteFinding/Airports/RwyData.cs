using CommonLibrary.LibraryExtension;

namespace QSP.RouteFinding.Airports
{
    public class RwyData : IRwyData
    {
        public string RwyIdent { get; private set; }
        public string Heading { get; private set; }
        public int LengthFt { get; private set; }
        public int WidthFt { get; private set; }
        public bool HasIlsInfo { get; private set; }

        // The following 3 values can be used only if HasIlsInfo is true.
        public bool IlsAvail { get; private set; }
        public string IlsFreq { get; private set; }
        public string IlsHeading { get; private set; }

        public double Lat { get; private set; }
        public double Lon { get; private set; }
        public int ElevationFt { get; private set; }
        public double GlideslopeAngle { get; private set; }
        public int ThresholdOverflyHeight { get; private set; }
        public string SurfaceType { get; private set; }
        public int RwyStatus { get; private set; }

        public RwyData(
            string RwyIdent,
            string Heading,
            int LengthFt,
            int WidthFt,
            bool HasIlsInfo,
            bool IlsAvail,
            string IlsFreq,
            string IlsHeading,
            double Lat,
            double Lon,
            int ElevationFt,
            double GlideslopeAngle,
            int ThresholdOverflyHeight,
            string SurfaceType,
            int RwyStatus)
        {
            this.RwyIdent = RwyIdent;
            this.Heading = Heading;
            this.LengthFt = LengthFt;
            this.WidthFt = WidthFt;
            this.HasIlsInfo = HasIlsInfo;
            this.IlsAvail = IlsAvail;
            this.IlsFreq = IlsFreq;
            this.IlsHeading = IlsHeading;
            this.Lat = Lat;
            this.Lon = Lon;
            this.ElevationFt = ElevationFt;
            this.GlideslopeAngle = GlideslopeAngle;
            this.ThresholdOverflyHeight = ThresholdOverflyHeight;
            this.SurfaceType = SurfaceType;
            this.RwyStatus = RwyStatus;
        }

        public bool Equals(IRwyData other)
        {
            return other != null &&
                RwyIdent == other.RwyIdent &&
                Heading == other.Heading &&
                LengthFt == other.LengthFt &&
                WidthFt == other.WidthFt &&
                HasIlsInfo == other.HasIlsInfo &&
                IlsAvail == other.IlsAvail &&
                IlsFreq == other.IlsFreq &&
                IlsHeading == other.IlsHeading &&
                Lat == other.Lat &&
                Lon == other.Lon &&
                ElevationFt == other.ElevationFt &&
                GlideslopeAngle == other.GlideslopeAngle &&
                ThresholdOverflyHeight == other.ThresholdOverflyHeight &&
                SurfaceType == other.SurfaceType &&
                RwyStatus == other.RwyStatus;
        }

        public override int GetHashCode()
        {
            return new object[]
            {
                RwyIdent,
                Heading,
                LengthFt,
                WidthFt,
                HasIlsInfo,
                IlsAvail,
                IlsFreq,
                IlsHeading,
                Lat,
                Lon,
                ElevationFt,
                GlideslopeAngle,
                ThresholdOverflyHeight,
                SurfaceType,
                RwyStatus
            }.HashCodeByElem();
        }
    }
}
