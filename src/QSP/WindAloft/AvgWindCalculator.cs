using QSP.MathTools.Vectors;
using QSP.RouteFinding.Data.Interfaces;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.Numbers;
using static QSP.MathTools.EarthGeometry;
using static QSP.MathTools.Integration;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static QSP.WindAloft.GroundSpeedCalculation;

namespace QSP.WindAloft
{
    public class AvgWindCalculator
    {
        private static readonly Vector3D DefaultLocation = LatLonToVector3D(0.0, 0.0);

        public double Ktas { get; }
        public double AltitudeFt { get; }

        private readonly IWindTableCollection windData;
        private Vector3D v1;
        private Vector3D v2;

        public AvgWindCalculator(IWindTableCollection windData, double Ktas, double AltitudeFt)
        {
            this.windData = windData;
            this.Ktas = Ktas;
            this.AltitudeFt = AltitudeFt;

            v1 = DefaultLocation;
            v2 = DefaultLocation;
        }

        // delta: in degrees
        public double GetAirDistance(ICoordinate point1, ICoordinate point2, double delta = 1.0)
        {
            if (point1.LatLonEquals(point2, 1E-5)) return 0.0;

            v1 = point1.ToVector3D();
            v2 = point2.ToVector3D();

            double deltaAlpha = ToRadian(delta);

            // Total distance
            double r = EarthRadiusNm * SafeAcos(v1.Dot(v2));

            // Total time required
            double time = Integrate(GetOneOverGS, 0.0, r, deltaAlpha * EarthRadiusNm);

            return time * Ktas;
        }

        // Returns 1.0/(ground speed in knots).
        private double GetOneOverGS(double r)
        {
            var v = GetV(v1, v2, r / EarthRadiusNm);
            return 1.0 / GetGS(windData, AltitudeFt, Ktas, v1, v2, v);
        }
    }
}
