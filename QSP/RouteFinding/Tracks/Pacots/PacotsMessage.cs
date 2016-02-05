using QSP.RouteFinding.Tracks.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsMessage : TrackMessage
    {
        private static readonly string HeaderKZAK = "KZAK OAKLAND OCA/FIR";
        private static readonly string HeaderRJJJ = "RJJJ FUKUOKA/JCAB AIR TRAFFIC FLOW MANAGEMENT CENTRE";

        private List<string> tracksKZAK;
        private List<string> tracksRJJJ;

        public PacotsMessage()
        {
            tracksKZAK = new List<string>();
            tracksRJJJ = new List<string>();
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
            index = htmlFile.IndexOf("The following are");
            Header = htmlFile.Substring(index, htmlFile.IndexOf("</", index) - index);

            //get the time stamp
            TimeStamp = GetTimeStamp(htmlFile, index);

            //KZAK part comes before RJJJ
            //goes to a line like this: "KZAK   OAKLAND OCA/FIR"
            int indexKZAK = htmlFile.IndexOf("OAKLAND OCA/FIR", index);
            int indexRJJJ = htmlFile.IndexOf("FUKUOKA/JCAB AIR TRAFFIC FLOW MANAGEMENT CENTRE", index);

            if (indexKZAK < indexRJJJ)
            {
                //get track definition message (TDM) for KZAK part
                tracksKZAK = getTdm(htmlFile, indexKZAK, indexRJJJ, true);

                //get RJJJ part
                tracksRJJJ = getTdm(htmlFile, indexRJJJ, htmlFile.Length - 1, false);
            }
            else
            {
                //get RJJJ part
                tracksRJJJ = getTdm(htmlFile, indexRJJJ, indexKZAK, false);

                //get track definition message (TDM) for KZAK part
                tracksKZAK = getTdm(htmlFile, indexKZAK, htmlFile.Length - 1, true);
            }
        }

        private List<string> getTdm(string htmlFile,
                                    int startIndex,
                                    int endIndex,
                                    bool isWestbound)
        {
            var tracks = new List<string>();
            int index = startIndex;
            string searchWord = isWestbound ? "(TDM TRK" : "EASTBOUND";

            while (true)
            {
                index = htmlFile.IndexOf(searchWord, index);

                if (index >= 0 && index <= endIndex)
                {
                    int end = htmlFile.IndexOf("</", index);

                    if (end < 0)
                    {
                        end = htmlFile.Length - 1;
                    }

                    tracks.Add(htmlFile.Substring(index, end - index));
                    index = end;
                }
                else
                {
                    return tracks;
                }
            }
        }

        private string GetTimeStamp(string htmlFile, int index)
        {
            try
            {
                index = htmlFile.IndexOf("Data Current as of:", index);
                index = htmlFile.IndexOf('>', index) + 1;
                return htmlFile.Substring(index, htmlFile.IndexOf("</", index));
            }
            catch
            {
                return "";
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
            var doc = new XElement(
                "Content", new XElement[]{
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