namespace QSP.WindAloft
{
    public struct WindUV
    {
        public double UComp { get; private set; }
        public double VComp { get; private set; }

        public WindUV(double u, double v)
        {
            UComp = u;
            VComp = v;
        }
    }
}
