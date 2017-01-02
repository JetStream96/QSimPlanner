using QSP.LibraryExtension.XmlSerialization;
using System;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.UI.Forms
{
    public class WindowSize
    {
        public static readonly string FileLocation = @"SavedStates\Window.xml";

        public bool Maximized { get; private set; }
        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }

        public WindowSize(bool Maximized, int WindowWidth, int WindowHeight)
        {
            this.Maximized = Maximized;
            this.WindowWidth = WindowWidth;
            this.WindowHeight = WindowHeight;
        }

        public static WindowSize Default => new WindowSize(false, 1300, 900);

        public class Serializer : IXSerializer<WindowSize>
        {
            public XElement Serialize(WindowSize w, string name)
            {
                return new XElement(name, new XElement[]
                {
                    w.Maximized.Serialize("Maximized"),
                    w.WindowWidth.Serialize("WindowWidth"),
                    w.WindowHeight.Serialize("WindowHeight")
                });
            }

            public WindowSize Deserialize(XElement item)
            {
                var d = Default;

                Action[] actions =
                {
                    () => d.Maximized = item.GetBool("Maximized"),
                    () => d.WindowWidth =item.GetInt("WindowWidth"),
                    () => d.WindowHeight = item.GetInt("WindowHeight")
                };

                foreach (var a in actions)
                {
                    IgnoreException(a);
                }

                return d;
            }
        }
    }
}
