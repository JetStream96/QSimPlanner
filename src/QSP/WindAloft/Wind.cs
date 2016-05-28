using System;
using static QSP.MathTools.Modulo;

namespace QSP.WindAloft
{
    public class Wind
    {
        private double _direction; // 0 - 360 deg
        private double _speed;     // knots
        
        public Wind(double Direction, double Speed)
        {
            this.Direction = Direction;
            this.Speed = Speed;
        }

        /// <summary>
        /// The direction of the wind. This value is larger than zero and no greater than 360.
        /// </summary>
        public double Direction
        {
            get
            {
                return _direction;
            }

            set
            {
                _direction = value.Mod(360);
            }
        }

        public double Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if (value >= 0.0)
                {
                    _speed = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        "Wind speed cannot be negative.");
                }
            }
        }

        public static Wind FromUV(WindUV w)
        {
            return FromUV(w.UComp, w.VComp);
        }

        public static Wind FromUV(double uWind, double vWind)
        {
            var Direction =
                 -Math.Atan2(vWind, uWind) / Math.PI * 180.0 + 90 + 180;
            var Speed = Math.Sqrt(uWind * uWind + vWind * vWind);
            return new Wind(Direction, Speed);
        }
        
        public string DirectionString()
        {
            return ((int)_direction).ToString().PadLeft(3, '0');
        }
    }
}
