using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace ServerCore
{
    public static class Util
    {
        public static string MapPath(IHostingEnvironment env, string path)
        {
            return Path.Combine(env.ContentRootPath, path);
        }
        
        // @NoThrow
        public static void TryAndLogIfFail(SharedData s, Action a)
        {
            try
            {
                a();
            }
            catch (Exception e)
            {
                s.Logger.Log(e.ToString());
            }
        }

        public static string AddTimeStamp(string msg)
        {
            return DateTime.UtcNow.ToString() + "  " + msg;
        }
    }
}
