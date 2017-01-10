using static System.Math;
using static QSP.MathTools.Numbers;
using static QSP.MathTools.Modulo;

namespace QSP.WindAloft
{
    public struct Wind
    {
        // The direction of the wind. This value is larger than 
        // zero and no greater than 360.
        public double Direction { get; private set; }

        // In knots
        public double Speed { get; private set; }

        public Wind(double Direction, double Speed)
        {
            this.Direction = Direction;
            this.Speed = Speed;
        }

        public static Wind FromUV(WindUV w)
        {
            return FromUV(w.UComp, w.VComp);
        }

        public static Wind FromUV(double uWind, double vWind)
        {
            var Direction = -Atan2(vWind, uWind) / PI * 180.0 + 90 + 180;
            var Speed = Sqrt(uWind * uWind + vWind * vWind);
            return new Wind(FitRange(Direction), Speed);
        }

        private static double FitRange(double direction)
        {
            direction = direction.Mod(360.0);
            return direction == 0.0 ? 360.0 : direction;
        }

        public string DirectionString()
        {
            return RoundToInt(Direction).ToString().PadLeft(3, '0');
        }
    }
}
