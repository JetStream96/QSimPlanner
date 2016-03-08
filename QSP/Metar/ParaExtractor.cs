namespace QSP.Metar
{
    public static class ParaExtractor
    {
        // TODO: Replace with regex.
        public static string WindInfo(string metar)
        {
            //will return "02013KT", or "14007MPS" if found 
            //return "NA" if nothing is found
            bool meetCondition = false;

            try
            {
                if (metar.IndexOf("VRB") != -1)
                {
                    return "36000KT";
                }

                for (int i = 0; i <= metar.Length - 10; i++)
                {
                    meetCondition = true;
                    for (int j = 0; j <= 4; j++)
                    {
                        if (!char.IsDigit(metar[i + j]))
                        {
                            meetCondition = false;
                            break;
                        }
                    }

                    if (meetCondition)
                    {
                        if (metar[i + 5] == 'G' && char.IsDigit(metar[i + 6]) && char.IsDigit(metar[i + 7]))
                        {
                            if (metar[i + 8] == 'K' && metar[i + 9] == 'T')
                            {
                                return metar.Substring(i, 5) + metar.Substring(i + 8, 2);
                            }
                            else if (metar[i + 8] == 'M' && metar[i + 9] == 'P' && metar[i + 10] == 'S')
                            {
                                return metar.Substring(i, 5) + metar.Substring(i + 8, 3);
                            }
                        }
                        else
                        {
                            if (metar[i + 5] == 'K' && metar[i + 6] == 'T')
                            {
                                return metar.Substring(i, 7);
                            }
                            else if (metar[i + 5] == 'M' && metar[i + 6] == 'P' & metar[i + 7] == 'S')
                            {
                                return metar.Substring(i, 8);
                            }
                        }
                    }
                }
            }
            catch
            {
                return "NA";
            }
            return "NA";
        }

        public static string TempInfo(string metar)
        {
            //input can be 12/09, 01/M01, M04/M06,etc
            //return "12/09", "-4/-6", or NA
            try
            {
                for (int i = 0; i <= metar.Length - 8; i++)
                {

                    if (metar[i] == ' ' && char.IsDigit(metar[i + 1]) && char.IsDigit(metar[i + 2]) &&
                        metar[i + 3] == '/' && char.IsDigit(metar[i + 4]) && char.IsDigit(metar[i + 5]))
                    {
                        return metar.Substring(i + 1, 5);

                    }
                    else if (metar[i] == ' ' && char.IsDigit(metar[i + 1]) && char.IsDigit(metar[i + 2]) && metar[i + 3] == '/' &&
                        metar[i + 4] == 'M' && char.IsDigit(metar[i + 5]) && char.IsDigit(metar[i + 6]))
                    {
                        return metar.Substring(i + 1, 6).Replace('M', '-');

                    }
                    else if (metar[i] == ' ' && metar[i + 1] == 'M' && char.IsDigit(metar[i + 2]) == true &&
                      char.IsDigit(metar[i + 3]) && metar[i + 4] == '/' && metar[i + 5] == 'M' &&
                     char.IsDigit(metar[i + 6]) && char.IsDigit(metar[i + 7]))
                    {
                        return metar.Substring(i + 1, 7).Replace('M', '-');
                    }
                }
            }
            catch
            {
                return "NA";
            }
            return "NA";
        }

        public static string PressInfo(string metar)
        {
            //will return, e.g. Q1013, A3000
            //return "NA" if nothing is found
            bool meetCondition = false;
            try
            {
                for (int i = 0; i <= metar.Length - 5; i++)
                {
                    meetCondition = true;
                    if (metar[i] == 'Q' || metar[i] == 'A')
                    {
                        for (int j = 1; j <= 4; j++)
                        {
                            if (!char.IsDigit(metar[i + j]))
                            {
                                meetCondition = false;
                                break;
                            }
                        }

                        if (meetCondition == true)
                        {
                            return metar.Substring(i, 5);
                        }
                    }
                }
            }
            catch
            {
                return "NA";
            }
            return "NA";
        }
    }
}
