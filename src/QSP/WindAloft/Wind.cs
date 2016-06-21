using System;

namespace QSP.WindAloft
{
    public class Wind
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

        public static Wind FromUv(WindUV w)
        {
            return FromUv(w.UComp, w.VComp);
        }

        public static Wind FromUv(double uWind, double vWind)
        {
            var Direction =
                 -Math.Atan2(vWind, uWind) / Math.PI * 180.0 + 90 + 180;
            var Speed = Math.Sqrt(uWind * uWind + vWind * vWind);
            return new Wind(Direction, Speed);
        }

        public string DirectionString()
        {
            return ((int)Direction).ToString().PadLeft(3, '0');
        }
    }
}
