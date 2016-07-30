using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;

namespace QSP.RouteFinding.Tracks.Pacots
{
    // TODO: Add unit test.
    public class PacotsMessage : TrackMessage
    {
        // Westbound tracks
        private static readonly string HeaderKzak =
            "KZAK OAKLAND OCA/FIR";

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
                ParseHtml(htmlFile);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    "Unable to parse the message.", ex);
            }
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

            var matches = Regex.Matches(
                source, pattern, RegexOptions.Singleline);

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

            var match = Regex.Match(
                htmlSource, pattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Value : null;
        }

        public override void LoadFromXml(XDocument doc)
        {
            var root = doc.Root;
            Header = doc.Element("Header").Value;
            TimeStamp = doc.Element("TimeStamp").Value;

            var elemKzak = doc.Element("KZAK").Elements("Track");
            WestboundTracks = elemKzak.Select(i => i.Value);

            var elemRjjj = doc.Element("RJJJ").Elements("Track");
            EastboundTracks = elemRjjj.Select(i => i.Value);
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendLine(Header);
            s.AppendLine("Data Current as of" + TimeStamp);
            s.AppendLine(GetStringTracks(HeaderKzak, WestboundTracks));
            s.AppendLine(GetStringTracks(HeaderRjjj, EastboundTracks));

            return s.ToString();
        }

        public override XDocument ToXml()
        {
            var doc = new XElement(
                "Content", new XElement[]{
                    new XElement("TrackSystem","PACOTs"),
                    new XElement("Header",Header),
                    new XElement("TimeStamp",TimeStamp),
                    new XElement("KZAK",GetXElement(WestboundTracks)),
                    new XElement("RJJJ",GetXElement(EastboundTracks))});

            return new XDocument(doc);
        }

        private static XElement[] GetXElement(IEnumerable<string> tracks)
        {
            return tracks.Select(s => new XElement("Track", s)).ToArray();
        }

        private static string GetStringTracks(
            string header, IEnumerable<string> tracks)
        {
            var s = new StringBuilder();
            s.AppendLine(header);
            tracks.ForEach(i => s.AppendLine(i));
            return s.ToString();
        }
    }
}