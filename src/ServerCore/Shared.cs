using LibraryExtension;
using LibraryExtension.Sets;

namespace ServerCore
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