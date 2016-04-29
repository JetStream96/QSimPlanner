using System;

namespace QSP.Utilities
{
    public static class LoggerInstance
    {
        private static Logger logger = new Logger();

        public static void WriteToLog(Exception ex)
        {
            logger.WriteToLog(ex);
        }

        public static void WriteToLog(string msg)
        {
            logger.WriteToLog(msg);
        }
    }
}
