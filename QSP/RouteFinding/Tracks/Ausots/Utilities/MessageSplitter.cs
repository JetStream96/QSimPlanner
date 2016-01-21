using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace QSP.RouteFinding.Tracks.Ausots.Utilities
{
    // Split the AusotsMessage into several strings, each of which represents a track which is 
    // ready to be passed into IndividualAusotsParser.
    //
    // For any given string, the Parse() method returns all substrings which starts with "TDM TRK"
    // and contains all lines until an "empty line"*.
    //
    // * An "empty line" is a line contains nothing but ' ', '\n', '\r', '\t'.

    public class MessageSplitter
    {
        private string AllTxt;

        public MessageSplitter(string MessageAllTxt)
        {
            AllTxt = MessageAllTxt;
        }

        public List<string> Split()
        {
            var result = new List<string>();
            int index = 0;

            while (true)
            {
                index = AllTxt.IndexOf("TDM TRK", index);

                if (index < 0)
                {
                    return result;
                }
                int next = IndexBeforeEmptyLine(AllTxt, index);
                result.Add(AllTxt.Substring(index, next - index + 1));
                index = next;
            }
        }

        private static int IndexBeforeEmptyLine(string item, int index)
        {
            while (CurrentLineIsEmpty(item, index) == false)
            {
                index = item.IndexOf('\n', index);
                if (index < 0)
                {
                    return item.Length - 1;
                }
                index++;
            }
            return index - 1;
        }

        private static bool CurrentLineIsEmpty(string item, int index)
        {
            int x = item.IndexOf('\n', index);

            if (x < 0)
            {
                x = item.Length - 1;
            }

            for (int i = index; i < x; i++)
            {
                if (DelimiterWords.Contains(item[i]) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
