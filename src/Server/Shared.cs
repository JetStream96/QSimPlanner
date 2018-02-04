using CommonLibrary;
using System.IO;

namespace Server
{
    public static class Shared
    {
        public static readonly LockedObj<string> UnloggedError = new LockedObj<string>("");
        public static readonly LockedObj<AntiSpamList> AntiSpam =
            new LockedObj<AntiSpamList>(new AntiSpamList());

        // Thread-safe
        public static readonly ErrorReportWriter ErrReportWriter = new ErrorReportWriter();
        public static readonly Logger Logger = new Logger();


        // Readonly
        public static readonly string ConfigFile =
#if DEBUG
            "~/App_Data/config_debug.xml";
#else
            "~/App_Data/config.xml";
#endif

        public static readonly object StatsFileLock = new object();
        public static readonly object NatsFileLock = new object();

        public static readonly string NatsDir = "~/App_Data/nats";
        public static readonly string WestNatsFile = Path.Combine(NatsDir, "Westbound.xml");
        public static readonly string EastNatsFile = Path.Combine(NatsDir, "Eastbound.xml");

        public static readonly string UpdateInfoFile = "~/App_Data/updates/info.xml";
    }
}