using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.StringParser
{
    public static class Utilities
    {
        public static int ParseInt(string item, int startindex, int endIndex)
        {
            int result = 0;
            short negate = 1;

            for (int i = startindex; i <= endIndex; i++)
            {
                if (item[i] >= '0' && item[i] <= '9')
                {
                    result *= 10;
                    result += item[i] - '0';
                }
                else if (item[i] == '-' && i==startindex )
                {
                    negate = -1;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            return result * negate;
        }

        public static double ParseDouble(string item, int startindex, int endIndex)
        {
            double result = 0.0;
            short negate = 1;

            for (int i = startindex; i <= endIndex; i++)
            {
                if (item[i] >= '0' && item[i] <= '9')
                {
                    result *= 10;
                    result += item[i] - '0';
                }
                else if (item[i] == '-' && i == startindex )
                {
                    negate = -1;
                }
                else if (item[i] == '.')
                {
                    double dec = 0.0;

                    for (int j = endIndex; j > i; j--)
                    {
                        if (item[j] > '9' || item[j] < '0')
                        {
                            throw new ArgumentException();
                        }
                        dec *= 0.1;
                        dec += item[j] - '0';
                    }
                    result += 0.1 * dec;
                    break;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            return result * negate;
        }
    }
}
