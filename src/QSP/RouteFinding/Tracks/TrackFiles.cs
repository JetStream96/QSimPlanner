using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks
{
    public static class TrackFiles
    {    
        public static void SaveToFile(IEnumerable<ITrackMessageNew> messages,
            string pathToFile)
        {
            var elem = messages.Select(m => m.ToXml().Root);
            var doc = new XDocument(new XElement("Root", elem.ToArray()));
            File.WriteAllText(pathToFile, doc.ToString());
        }

        public static IEnumerable<XDocument> ReadFromFile(string path)
        {
            var root = XDocument.Load(path).Root;
            return root.Elements().Select(e => new XDocument(e));
        }
    }
}
