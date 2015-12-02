using System;
using System.Collections.Generic;
using QSP.MathTools;
using QSP.LibraryExtension;
using QSP.Core;

namespace QSP
{

    public class FlightTimeCalculator
    {
        private int[,] time_req_table;

        public FlightTimeCalculator(string sourceTxt)
        {
            try
            {
                time_req_table = importFlightTimeTable(sourceTxt);
            }
            catch
            {
                throw new InvalidAircraftDatabaseException();
            }

        }

        public int GetTimeMin(double airDistance)
        {
            //AC_time_req_file_text is the original text of the table
            //air_dis is in the unit of nm
            //options: 
            //  "MIN", will return, e.g. 156     (156 min)
            //  "HHMM", will return, e.g. 0236   (2 hours and 36 min)

            int m = time_req_table.GetLength(1) - 2;
            for (int k = 1; k <= time_req_table.GetLength(1) - 1; k++)
            {
                if (airDistance <= time_req_table[0, k])
                {
                    m = k - 1;
                    break;
                }
            }
            return (int)(Interpolation.Interpolate(time_req_table[0, m], time_req_table[0, m + 1], airDistance,
                                                   time_req_table[1, m], time_req_table[1, m + 1]));
        }

        private int[,] importFlightTimeTable(string sourceTxt)
        {

            string[] all_lines = sourceTxt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> allLines = new List<string>();

            //only consider the lines starting with a number
            string[] t = null;
            for (int i = 0; i < all_lines.Length; i++)
            {
                t = all_lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (t != null && char.IsDigit(t[0][0]))
                {
                    allLines.Add(all_lines[i]);
                }
            }

            //import to a table
            int[,] time_req_table = new int[2, allLines.Count];

            for (int j = 0; j < allLines.Count; j++)
            {
                t = allLines[j].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                time_req_table[0, j] = Convert.ToInt32(t[0]);
                //e.g. 500
                time_req_table[1, j] = TimeFormat.HH_Colon_MMToMin(t[t.Length - 1]);
                //e.g. 1:43 -> 103
            }

            return time_req_table;

        }

    }
}
