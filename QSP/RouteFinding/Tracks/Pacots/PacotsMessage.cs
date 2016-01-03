using System;
using System.Collections.Generic;
using QSP.LibraryExtension;
using System.Collections.ObjectModel;
using static QSP.LibraryExtension.Strings;

namespace QSP.RouteFinding.Tracks.Pacots
{

    public class PacotsMessage
    {
        private List<string> tracksKZAK;
        private List<string> tracksRJJJ;

        public PacotsMessage(string htmlFile)
        {
            tracksKZAK = new List<string>();
            tracksRJJJ = new List<string>();

            try
            {
                ParseHtml(htmlFile);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse the message.", ex);
            }
        }

        public ReadOnlyCollection<string> WestboundTracks
        {
            get { return tracksKZAK.AsReadOnly(); }
        }

        public ReadOnlyCollection<string> EastboundTracks
        {
            get { return tracksRJJJ.AsReadOnly(); }
        }

        public string TimeStamp { get; private set; }
        public string GeneralMsg { get; private set; }

        private void ParseHtml(string htmlFile)
        {
            int index = 0;

            //get the general message
            GeneralMsg = Strings.StringStartEndWith(htmlFile, "The following are", "</tt>", CutStringOptions.PreserveStart);

            //get the time stamp
            var timeInfo = GetTimeStamp(htmlFile, index);
            TimeStamp = timeInfo.Item1;
            index = timeInfo.Item2;

            //KZAK part comes before RJJJ
            //goes to a line like this: "KZAK   OAKLAND OCA/FIR"
            int KZAK_Part_Index = htmlFile.IndexOf("OAKLAND OCA/FIR", index);
            int RJJJ_Part_Index = htmlFile.IndexOf("FUKUOKA/JCAB AIR TRAFFIC FLOW MANAGEMENT CENTRE", index);

            if (KZAK_Part_Index < RJJJ_Part_Index)
            {
                //get track definition message (TDM) for KZAK part
                getTDM(htmlFile, KZAK_Part_Index, RJJJ_Part_Index - 1, 0);

                //get RJJJ part
                getTDM(htmlFile, RJJJ_Part_Index, htmlFile.Length - 1, 1);
            }
            else
            {
                //get RJJJ part
                getTDM(htmlFile, RJJJ_Part_Index, KZAK_Part_Index - 1, 1);

                //get track definition message (TDM) for KZAK part
                getTDM(htmlFile, KZAK_Part_Index, htmlFile.Length - 1, 0);
            }
        }

        private void getTDM(string htmlFile, int startIndex, int endIndex, int part)
        {
            int index = startIndex;
            string findTarget = null;
            List<string> trackList = null;

            if (part == 0)
            {
                findTarget = "(TDM TRK";
                trackList = tracksKZAK;
            }
            else
            {
                findTarget = "EASTBOUND";
                trackList = tracksRJJJ;
            }

            while (index <= endIndex)
            {
                index = htmlFile.IndexOf(findTarget, index);

                if (index >= 0)
                {
                    int nextIndex = htmlFile.IndexOf("</PRE>", index);
                    string tdm = htmlFile.Substring(index, nextIndex - index);
                    trackList.Add(tdm);
                    index = nextIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private Pair<string, int> GetTimeStamp(string htmlFile, int startIndex)
        {
            try
            {
                var index1 = htmlFile.IndexOf("Data Current as of", startIndex);
                var index2 = htmlFile.IndexOf("</span>", index1);

                var ignore1 = htmlFile.IndexOf("<", index1);
                var ignore2 = htmlFile.IndexOf(">", ignore1);

                return new Pair<string, int>(htmlFile.Substring(index1, ignore1 - index1) + htmlFile.Substring(ignore2 + 1, index2 - ignore2 - 1), index2);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to find the time stamp.", ex);
            }
        }
    }
}
