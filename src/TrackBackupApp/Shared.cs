using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QSP.LibraryExtension;
using TrackBackupApp.Stats;

namespace TrackBackupApp
{
    public static class Shared
    {
        public static LockedObj<string> UnloggedError = new LockedObj<string>("");
        public static LockedObj<AntiSpamList> AntiSpam =
            new LockedObj<AntiSpamList>(new AntiSpamList());
        public static LockedObj<Statistics> Stats =
            new LockedObj<Statistics>(Helpers.LoadOrGenerateFile());

        // Thread-safe 
        public static readonly ErrorReportWriter ErrReportWriter = new ErrorReportWriter();
        public static readonly Logger Logger = new Logger();
    }
}