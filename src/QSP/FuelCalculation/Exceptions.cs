using System;
using System.Runtime.Serialization;

namespace QSP.FuelCalculation
{
    [Serializable]
    public class InvalidPlanAltitudeException : Exception
    {
        public InvalidPlanAltitudeException()
        {
        }

        public InvalidPlanAltitudeException(string message) : base(message)
        {
        }

        public InvalidPlanAltitudeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidPlanAltitudeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}