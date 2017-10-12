using QSP.RouteFinding.Tracks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CommonLibrary.LibraryExtension;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsMessage : ITrackMessage
    {
        // Westbound tracks
        private static readonly string HeaderKzak = "KZAK OAKLAND OCA/FIR";

        // Eastbound tracks
        private static readonly string HeaderRjjj =
            "RJJJ FUKUOKA/JCAB AIR TRAFFIC FLOW MANAGEMENT CENTRE";

        public IEnumerable<string> WestboundTracks { get; private set; }
        public IEnumerable<string> EastboundTracks { get; private set; }
        public string TimeStamp { get; private set; }
        public string Header { get; private set; }

        public PacotsMessage()
        {
            WestboundTracks = new string[0];
            EastboundTracks = new string[0];
            TimeStamp = "";
            Header = "";
        }

        public PacotsMessage(
            IEnumerable<string> WestboundTracks,
            IEnumerable<string> EastboundTracks,
            string TimeStamp,
            string Header)
        {
            this.WestboundTracks = WestboundTracks;
            this.EastboundTracks = EastboundTracks;
            this.TimeStamp = TimeStamp;
            this.Header = Header;
        }

        public PacotsMessage(string htmlFile) : this()
        {
            try
            {
                ParseHtml(htmlFile.RemoveIllegalXmlChar());
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse the message.", ex);
            }
        }
        
        public PacotsMessage(XDocument doc)
        {
            var root = doc.Root;
            Header = root.Element("Header").Value;
            TimeStamp = root.Element("TimeStamp").Value;

            var elemKzak = root.Element("KZAK").Elements("Track");
            WestboundTracks = elemKzak.Select(i => i.Value);

            var elemRjjj = root.Element("RJJJ").Elements("Track");
            EastboundTracks = elemRjjj.Select(i => i.Value);
        }


        private void ParseHtml(string htmlSource)
        {
            Header = GetHeader(htmlSource) ?? "";
            TimeStamp = GetTimeStamp(htmlSource) ?? "";
            WestboundTracks = GetTracks(htmlSource, true);
            EastboundTracks = GetTracks(htmlSource, false);
        }

        private IEnumerable<string> GetTracks(string source, bool isWestBound)
        {
            var pattern = isWestBound ?
                @"(\(TDM TRK.*?)</" :
                @"(EASTBOUND PACOTS TRACKS.*?)</";

            var matches = Regex.Matches(source, pattern, RegexOptions.Singleline);

            return matches.Cast<Match>().Select(m => m.Groups[1].Value);
        }

        // Returns null if no match is found.
        private static string GetHeader(string htmlSource)
        {
            var match = Regex.Match(htmlSource, @"(The following are.*?)</");

            if (match.Success)
            {
                return WebUtility.HtmlDecode(match.Groups[1].Value);
            }

            return null;
        }

        // Returns null if no match is found.
        private string GetTimeStamp(string htmlSource)
        {
            // Try to match "Tue, 16 Feb 2016 14:44:00 GMT"
            var weekDays = @"(Mon|Tue|Wed|Thu|Fri|Sat|Sun)";
            var months = @"(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)";
            var pattern = weekDays + @",?\s*\d{1,2}\s+" + months +
                @"\s+\d{4}\s+\d{1,2}:\d{1,2}:\d{1,2}\s+GMT";

            var match = Regex.Match(htmlSource, pattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Value : null;
        }

        public override string ToString()
        {
            var s = new StringBuilder();

            s.Append(Header);
            s.Append("\n\nTime updated: " + TimeStamp);
            s.Append("\n\n" + HeaderKzak + "\n\n");
            s.Append(string.Join("\n", WestboundTracks));
            s.Append("\n\n" + HeaderRjjj + "\n\n");
            s.Append(string.Join("\n", EastboundTracks));

            return s.ToString();
        }

        public XDocument ToXml()
        {
            var doc = new XElement(
                "Content", new XElement[]{
                    new XElement("TrackSystem", TrackType.Pacots.TrackString()),
                    new XElement("Header", Header),
                    new XElement("TimeStamp", TimeStamp),
                    new XElement("KZAK", GetXElement(WestboundTracks)),
                    new XElement("RJJJ", GetXElement(EastboundTracks))});

            return new XDocument(doc);
        }

        private static XElement[] GetXElement(IEnumerable<string> tracks)
        {
            return tracks.Select(s => new XElement("Track", s)).ToArray();
        }        
    }
}