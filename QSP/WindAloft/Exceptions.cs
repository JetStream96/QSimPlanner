using System;

namespace QSP.WindAloft
{

    [Serializable]
    public class WindNotAvailException : Exception
    {
        public WindNotAvailException() { }
        public WindNotAvailException(string message) : base(message) { }
        public WindNotAvailException(string message, Exception inner) : base(message, inner) { }
        protected WindNotAvailException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
