using QSP.LibraryExtension;
using QSP.LibraryExtension.Sets;
using TrackBackupApp.Stats;

namespace TrackBackupApp
{
    public static class Shared
    {
        public static LockedObj<string> UnloggedError = new LockedObj<string>("");
        public static LockedObj<AntiSpamList> AntiSpam =
            new LockedObj<AntiSpamList>(new AntiSpamList());

        // Do not change the reference to Stats.Value.
        public static LockedObj<Statistics> Stats =
            new LockedObj<Statistics>(Helpers.LoadOrGenerateFile());

        // Thread-safe 
        public static readonly ErrorReportWriter ErrReportWriter = new ErrorReportWriter();
        public static readonly Logger Logger = new Logger();

        // Readonly
        public static readonly string ConfigFile =
#if DEBUG
            "~/config_debug.xml";
#else
            "~/config.xml";
#endif

        public static readonly IReadOnlySet<string> HiddenFileSet = 
            HiddenFiles.LoadFromFileAndLog();

        public static readonly object StatsFileLock = new object();
        public static readonly object NatsFileLock = new object();
    }
}