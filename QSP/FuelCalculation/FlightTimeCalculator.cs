using System;
using System.Collections.Generic;
using QSP.MathTools.Interpolation;
using QSP.LibraryExtension;
using QSP.Core;

namespace QSP
{
    public class FlightTimeCalculator
    {
        private int[,] TimeRequiredTable;

        public FlightTimeCalculator(string sourceTxt)
        {
            try
            {
                TimeRequiredTable = importFlightTimeTable(sourceTxt);
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

            int m = TimeRequiredTable.GetLength(1) - 2;
            for (int k = 1; k < TimeRequiredTable.GetLength(1); k++)
            {
                if (airDistance <= TimeRequiredTable[0, k])
                {
                    m = k - 1;
                    break;
                }
            }
            return (int)(Interpolate1D.Interpolate(
                TimeRequiredTable[0, m], TimeRequiredTable[0, m + 1], airDistance,
                TimeRequiredTable[1, m], TimeRequiredTable[1, m + 1]));
        }

        private int[,] importFlightTimeTable(string sourceTxt)
        {
            var lines = sourceTxt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var allLines = new List<string>();

            //only consider the lines starting with a number
            string[] t = null;
            for (int i = 0; i < lines.Length; i++)
            {
                t = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (t != null && char.IsDigit(t[0][0]))
                {
                    allLines.Add(lines[i]);
                }
            }

            //import to a table
            int[,] timeReqTable = new int[2, allLines.Count];

            for (int j = 0; j < allLines.Count; j++)
            {
                t = allLines[j].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                timeReqTable[0, j] = Convert.ToInt32(t[0]);
                //e.g. 500
                timeReqTable[1, j] = TimeFormat.HourColonMinToMin(t[t.Length - 1]);
                //e.g. 1:43 -> 103
            }

            return timeReqTable;
        }
    }
}
