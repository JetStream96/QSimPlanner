using QSP.LibraryExtension.StringParser;

namespace QSP.RouteFinding.Tracks.Pacots.Eastbound
{
    // Interpret this message:
    //
    //   TRACK 1.
    //    FLEX ROUTE : KALNA 42N160E 45N170E 47N180E 49N170W 50N160W 51N150W
    //                 51N140W ORNAI
    //   JAPAN ROUTE : ONION OTR5 KALNA
    //     NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU MARNR KSEA
    //                 ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
    //                 ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
    //           RMK : ACFT LDG OTHER DEST--ORNAI SIMLU KEPKO UPR TO DEST
    //
    public class Interpreter
    {
        private string text;

        public Interpreter(string text)
        {
            this.text = text;
        }

        public EastboundTrackStrings Parse()
        {
            var result = new EastboundTrackStrings();

            // ID
            var sp = new StringParser(text);
            sp.MoveToNextIndexOf("TRACK");
            sp.MoveToNextDigit();
            result.ID = sp.ParseInt();

            // Flex Route
            sp.MoveToNextIndexOf("FLEX ROUTE");
            sp.MoveToNextIndexOf(':');
            sp.CurrentIndex++;
            result.FlexRoute = sp.ReadString(
                indexBeforeNextLineWhichContainsColon(sp.CurrentIndex));
            sp.CurrentIndex++;

            int rmk = sp.NextIndexOf("RMK");
            if (rmk < 0)
            {
                result.Remark = "";
                rmk = text.Length;
            }
            else
            {
                result.Remark = getRmk(rmk);
            }
            result.ConnectionRoute = sp.ReadString(rmk - 1);

            return result;
        }

        private int indexBeforeNextLineWhichContainsColon(int index)
        {
            int x = text.IndexOf(':', index);

            if (x < 0)
            {
                return text.Length - 1;
            }

            x = text.LastIndexOf('\n', x);

            if (x < 0 || x < index)
            {
                return index;
            }
            return x;
        }

        private string getRmk(int index)
        {
            index = text.IndexOf(':', index);

            if (index < 0 || index + 1 > text.Length - 1)
            {
                return "";
            }
            return text.Substring(index + 1);
        }

        public class EastboundTrackStrings
        {
            public int ID;
            public string FlexRoute;
            public string ConnectionRoute;
            public string Remark;
        }
    }
}
