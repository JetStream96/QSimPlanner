using QSP.LibraryExtension;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QSP.Utilities
{
    public class Logger
    {
        private SequentialTaskRunner fileWriter = new SequentialTaskRunner();
        private string filePath;

        public Logger(string filePath = "Log.txt")
        {
            this.filePath = filePath;
        }

        public void WriteToLog(Exception ex)
        {
            WriteToLog(ex.ToString());
        }

        public void WriteToLog(string msg)
        {
            var task = new Task(() =>
              {
                  try
                  {
                      File.AppendAllText(
                          filePath,
                          DateTime.Now.ToString() + ":\n" + msg + "\n\n");
                  }
                  catch
                  { }
              });

            fileWriter.AddTask(task);
        }
    }
}
