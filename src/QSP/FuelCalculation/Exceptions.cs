using System;
using System.Runtime.Serialization;

namespace QSP.FuelCalculation
{
    [Serializable]
    public class ElevationDifferenceTooLargeException : Exception
    {
        public ElevationDifferenceTooLargeException() { }

        public ElevationDifferenceTooLargeException(string message) : base(message) { }

        public ElevationDifferenceTooLargeException(string message, Exception inner) : base(message, inner)
        { }

        protected ElevationDifferenceTooLargeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }
    }
}