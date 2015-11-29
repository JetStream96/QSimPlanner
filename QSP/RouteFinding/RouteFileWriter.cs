using System;
using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.Containers;
using System.IO;
using System.Windows.Forms;
using QSP.Core;

namespace QSP.RouteFinding
{
    public class RouteFileWriter
    {
        private string txtToWrite;
        private string origDest;
        private List<RouteExportCommand> commands;
        private List<string> FilesToWrite;

        public RouteFileWriter(string txtToWrite) : this(RouteFindingCore.RouteToDest, QspCore.AppSettings.ExportCommands, txtToWrite) { }

        public RouteFileWriter(Route route, List<RouteExportCommand> commands, string txtToWrite)
        {
            this.commands = commands;
            this.txtToWrite = txtToWrite;
            var orig = route.Waypoints.First().ID.Substring(0, 4);
            var dest = route.Waypoints.Last().ID.Substring(0, 4);
            origDest = orig + dest;
        }

        private void createBackup()
        {
            //a backup is generated to application folder
            Directory.CreateDirectory(QspCore.QspAppDataDirectory + "\\FPL");
            using (StreamWriter writer = new StreamWriter(QspCore.QspAppDataDirectory + "\\FPL\\" + origDest + ".rte"))
            {
                writer.Write(txtToWrite);
            }
        }

        private static string getFileExtension(string format)
        {
            //e.g. "PMDG" -> ".rte"
            switch (format)
            {
                case "PMDG":
                    return ".rte";
            }
            return null;
        }

        private string fileName(int num)
        {
            return origDest + num.ToString().PadLeft(2, '0');
        }

        private void findAppropriateFileName()
        {
            FilesToWrite = new List<string>();
            int num = 1;

            foreach (var i in commands)
            {
                if (i.FilePath != null && !string.IsNullOrEmpty(i.FilePath))
                {
                    try
                    {
                        string extension = getFileExtension(i.Format);

                        //e.g. "RCTPVHHH01.rte"
                        string fullFileName = i.FilePath + "\\" + fileName(num) + extension;

                        //if file exists already, change the filename, to "RCTPVHHH02"
                        while (File.Exists(fullFileName))
                        {
                            if (num == 99)
                            {
                                //change to some other exception
                                throw new ArgumentException();
                            }
                            else
                            {
                                num++;
                            }
                            fullFileName = i.FilePath + "\\" + fileName(num) + extension;
                        }

                        //file does not exist
                        //add to fileToWrite
                        FilesToWrite.Add(fullFileName);
                    }
                    catch
                    {
                        //change to some other exception
                        throw new ArgumentException();
                    }
                }
            }
        }

        private  void writeFiles()
        {
  //if nothing is going to be exported, i.e. user didn't select any export option, then abort
            if (FilesToWrite.Count == 0)
            {
                MessageBox.Show("No route file to be exported. Please select select export settings in options page.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var FailedExportPaths = new List<string>();

            foreach (var i in FilesToWrite)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(i))
                    {
                        writer.Write(txtToWrite);
                    }
                }
                catch (Exception ex)
                {
                    FailedExportPaths.Add(ex.Message.ToString());
                }

            }

            int NumOfErrors = FailedExportPaths.Count;

            if (NumOfErrors != 0)
            {
                string ErrorText = "Failed to export " + NumOfErrors + " file(s)." + Environment.NewLine;
                foreach (var j in FailedExportPaths)
                {
                    ErrorText += j + Environment.NewLine;
                }

                MessageBox.Show(ErrorText, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Company route " + FilesToWrite.First() + " exported.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public void Export()
        {
            createBackup();
            findAppropriateFileName();
            writeFiles();
        } 
    }
}
