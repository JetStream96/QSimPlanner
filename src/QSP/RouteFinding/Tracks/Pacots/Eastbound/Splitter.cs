using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension.StringParser;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.LibraryExtension.Lists;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Tracks.Pacots.Eastbound
{
    // Split this into 3 messages:
    //
    //   ...
    //   TRACK 1.
    //    FLEX ROUTE : KALNA 42N160E 45N170E 47N180E 49N170W 50N160W 51N150W
    //                 51N140W ORNAI
    //   JAPAN ROUTE : ONION OTR5 KALNA
    //     NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU MARNR KSEA
    //                 ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
    //                 ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
    //           RMK : ACFT LDG OTHER DEST--ORNAI SIMLU KEPKO UPR TO DEST
    //   TRACK 2.
    //    FLEX ROUTE : EMRON 38N160E 38N170E 38N180E 38N170W 38N160W 39N150W
    //                 40N140W 40N130W SHENU
    //   JAPAN ROUTE : ONION OTR5 ADNAP OTR7 EMRON
    //     NAR ROUTE : ACFT LDG KSFO--SHENU AMAKR BGGLO KSFO
    //                 ACFT LDG KLAX--SHENU ENI AVE FIM KLAX
    //   TRACK 3.
    //    FLEX ROUTE : LEPKI 37N160E 37N170E 36N180E 36N170W 36N160W 37N150W
    //                 38N140W 39N130W DACEM
    //   JAPAN ROUTE : AVBET OTR11 LEPKI
    //     NAR ROUTE : ACFT LDG KLAX--DACEM PAINT PIRAT AVE FIM KLAX
    //                 ACFT LDG KSFO--DACEM PAINT PIRAT OSI KSFO
    //           RMK : ATM CENTER TEL:81-92-608-8870. 06 FEB 07:00 2016 UNTIL 06 FEB
    //   21:00 2016. CREATED: 05 FEB 18:49 2016
    //
    public class Splitter
    {
        private string text;

        public Splitter(string text)
        {
            this.text = text;
        }

        public List<string> Split()
        {
            var result = new List<string>();
            var indices = GetIndices();

            if (indices.Count == 0)
            {
                return result;
            }

            for (int i = 0; i < indices.Count - 1; i++)
            {
                result.Add(text.Substring(indices[i], indices[i + 1] - indices[i]));
            }

            result.Add(text.Substring(indices.Last(), text.Length - indices.Last()));
            return result;
        }

        private List<int> GetIndices()
        {
            var indices = text.IndicesOf("TRACK");

            for (int i = indices.Count - 1; i >= 0; i--)
            {
                if (IsValid(indices[i]) == false)
                {
                    indices.RemoveAt(i);
                }
            }
            return indices;
        }

        private bool IsValid(int index)
        {
            var sp = new StringParser(text);
            sp.CurrentIndex = index;
            bool inBound = sp.MoveRight("TRACK".Length);

            if (inBound == false)
            {
                return false;
            }
            sp.SkipAny(DelimiterWords);
            char c = text[sp.CurrentIndex];
            return (c >= '0' && c <= '9');
        }
    }
}
