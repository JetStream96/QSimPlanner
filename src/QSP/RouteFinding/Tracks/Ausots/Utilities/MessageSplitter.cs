using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QSP.RouteFinding.Tracks.Ausots.Utilities
{
    /*
     Split the AusotsMessage into several strings, each of which represents 
     a track which is ready to be passed into IndividualAusotsParser.
    
     For any given string, the Parse() method returns all substrings which 
     starts with "TDM TRK" and contains all lines before an "empty line"*.
    
     See unit test for examples.

     * An "empty line" is a line contains nothing but ' ', '\r', '\t'.
    */

    public static class MessageSplitter
    {
        public static List<string> Split(string AllTxt)
        {
            var pattern = @"^\s*(TDM TRK.*?)(\r?\n[ \t\r]*(\n|\Z)|\Z)";
            //                              ^                   ^
            //                              matches an empty line

            var matches = Regex.Matches(AllTxt, pattern,
                RegexOptions.Singleline | RegexOptions.Multiline);

            return matches.Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .ToList();
        }        
    }
}
