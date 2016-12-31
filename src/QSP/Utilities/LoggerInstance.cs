using System;

namespace QSP.Utilities
{
    public static class LoggerInstance
    {
        private static Logger logger = new Logger();

        public static void Log(Exception ex) => logger.Log(ex);

        public static void Log(string msg)=> logger.Log(msg);
    }
}
