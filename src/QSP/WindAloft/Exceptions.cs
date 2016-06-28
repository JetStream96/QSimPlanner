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
    
    [Serializable]
    public class ReadWindFileException : Exception
    {
        public ReadWindFileException() { }
        public ReadWindFileException(string message) : base(message) { }
        public ReadWindFileException(string message, Exception inner) : base(message, inner) { }
        protected ReadWindFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
