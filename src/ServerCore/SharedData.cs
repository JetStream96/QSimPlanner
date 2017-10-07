using LibraryExtension;
using LibraryExtension.Sets;
using Microsoft.AspNetCore.Hosting;

namespace ServerCore
{
    public class SharedData
    {
        // Singleton
        private static SharedData instance;
        public static SharedData GetInstance(IHostingEnvironment env)
        {
            if (instance == null) instance = new SharedData(env);
            return instance;
        }


        public IHostingEnvironment HostingEnvironment { get; }
        public LockedObj<string> UnloggedError { get; } = new LockedObj<string>("");
        public LockedObj<AntiSpamList> AntiSpam { get; } =
            new LockedObj<AntiSpamList>(new AntiSpamList());

        // Thread-safe
        public ErrorReportWriter ErrReportWriter { get; private set; }
        public Logger Logger { get; private set; }

        // Readonly
        public static readonly string ConfigFile =
#if DEBUG
            "~/config_debug.xml";
#else
            "~/config.xml";
#endif

        public IReadOnlySet<string> HiddenFileSet { get; private set; }

        public object StatsFileLock { get; } = new object();
        public object NatsFileLock { get; } = new object();

        public SharedData(IHostingEnvironment env)
        {
            HostingEnvironment = env;
            ErrReportWriter = new ErrorReportWriter(env);
            Logger = new Logger(env);
            HiddenFileSet = HiddenFiles.LoadFromFileAndLog(env);
        }

        public string MapPath(string path) => Util.MapPath(HostingEnvironment, path);
    }
}