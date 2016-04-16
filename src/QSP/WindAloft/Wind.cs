using System;
using static QSP.MathTools.Modulo;

namespace QSP.WindAloft
{
    public class Wind
    {
        private double _direction; // 0 - 360 deg
        private double _speed;     // knots

        public Wind()
        {
            _direction = 0.0;
            _speed = 0.0;
        }

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
                    throw new ArgumentOutOfRangeException("Wind speed cannot be negative.");
                }
            }
        }

        public void SetUV(double uWind, double vWind)
        {
            Direction = -Math.Atan2(vWind, uWind) / Math.PI * 180 + 90 + 180;
            Speed = Math.Sqrt(uWind * uWind + vWind * vWind);
        }

        public string DirectionString()
        {
            return ((int)_direction).ToString().PadLeft(3, '0');
        }
    }
}
