using System;
using System.Collections.Generic;
using QSP.LibraryExtension;
using System.Collections.ObjectModel;
using static QSP.LibraryExtension.Strings;
using System.Xml.Linq;
using QSP.RouteFinding.Tracks.Common;
using System.Text;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsMessage : TrackMessage
    {
        private static string HeaderKZAK = "KZAK OAKLAND OCA/FIR";
        private static string HeaderRJJJ = "RJJJ FUKUOKA/JCAB AIR TRAFFIC FLOW MANAGEMENT CENTRE";

        private List<string> tracksKZAK;
        private List<string> tracksRJJJ;

        public PacotsMessage()
        {
            tracksKZAK = new List<string>();
            tracksRJJJ = new List<string>();
        }

        public PacotsMessage(XDocument doc)
        {
            LoadFromXml(doc);
        }

        public PacotsMessage(string htmlFile) : this()
        {
            try
            {
                ParseHtml(htmlFile);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse the message.", ex);
            }
        }

        #region Properties

        public ReadOnlyCollection<string> WestboundTracks
        {
            get { return tracksKZAK.AsReadOnly(); }
        }

        public ReadOnlyCollection<string> EastboundTracks
        {
            get { return tracksRJJJ.AsReadOnly(); }
        }

        public string TimeStamp { get; private set; }
        public string Header { get; private set; }

        #endregion

        private void ParseHtml(string htmlFile)
        {
            int index = 0;

            //get the general message
            Header = Strings.StringStartEndWith(htmlFile, "The following are", "</tt>", CutStringOptions.PreserveStart);

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

        public override void LoadFromXml(XDocument doc)
        {
            var root = doc.Root;
            Header = doc.Element("Header").Value;
            TimeStamp = doc.Element("TimeStamp").Value;

            tracksKZAK = new List<string>();
            tracksRJJJ = new List<string>();

            foreach (var i in doc.Element("KZAK").Elements("Track"))
            {
                tracksKZAK.Add(i.Value);
            }

            foreach (var i in doc.Element("RJJJ").Elements("Track"))
            {
                tracksRJJJ.Add(i.Value);
            }
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendLine(Header);
            s.AppendLine("Data Current as of" + TimeStamp);
            s.AppendLine(GetStringTracks(HeaderKZAK, tracksKZAK));
            s.AppendLine(GetStringTracks(HeaderRJJJ, tracksRJJJ));

            return s.ToString();
        }

        public override XDocument ToXml()
        {
            var doc = new XElement("Content", new XElement[]{
                                                             new XElement("TrackSystem","PACOTs"),
                                                             new XElement("Header",Header),
                                                             new XElement("TimeStamp",TimeStamp),
                                                             new XElement("KZAK",GetXElement(tracksKZAK)),
                                                             new XElement("RJJJ",GetXElement(tracksRJJJ))
                                                            });
            return new XDocument(doc);
        }

        private static XElement[] GetXElement(List<string> tracks)
        {
            var array = new XElement[tracks.Count];

            for (int i = 0; i < tracks.Count; i++)
            {
                array[i] = new XElement("Track", tracks[i]);
            }
            return array;
        }

        private static string GetStringTracks(string header, List<string> tracks)
        {
            var s = new StringBuilder();
            s.AppendLine(header);

            foreach (var i in tracks)
            {
                s.AppendLine(i);
            }
            return s.ToString();
        }
    }
}