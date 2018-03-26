namespace QSP.TOPerfCalculation
{
    public class TOReportRow
    {
        public double OatCelsius { get; }
        public double RwyRequiredMeter { get; }
        public double RwyRemainingMeter { get; }

        public TOReportRow(double OatCelsius, double RwyRequiredMeter, double RwyRemainingMeter)
        {
            this.OatCelsius = OatCelsius;
            this.RwyRequiredMeter = RwyRequiredMeter;
            this.RwyRemainingMeter = RwyRemainingMeter;
        }
    }
}
