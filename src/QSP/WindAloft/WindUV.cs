namespace QSP.WindAloft
{
    /// <summary>
    /// U, V are the x and y components of the wind velocity.
    /// </summary>
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
