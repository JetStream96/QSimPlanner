using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PayloadRangeChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PlotPayloadRangeChart();
        }

        private static double fuelCapacityTon = 20.894;
        private static double reserveFuelTon =
            (38.0 * 45.0 +  // Final reserve
            130.0)          // One go-around
            / 1000.0;

        private static double contingencyPercent = 5.0;

        private static double cruiseSpeedKnots = 460.0;

        private void PlotPayloadRangeChart()
        {
            double zfw = 40.0;

            while (zfw <= 55.0)
            {
                chart1.Series["Series1"].Points.AddXY(GetRangePara1(zfw), zfw);
                zfw += 0.1;
            }

            chart1.Series["Series1"].ChartType = SeriesChartType.FastLine;
            chart1.Series["Series1"].Color = Color.DodgerBlue;

            // Adjust these parameters according to data range.
            chart1.ChartAreas[0].AxisX.Maximum = 5000;
            chart1.ChartAreas[0].AxisX.Minimum = 3500;
            chart1.ChartAreas[0].AxisY.Maximum = 60;
            chart1.ChartAreas[0].AxisY.Minimum = 35;
        }

        private static double GetRangePara1(double zfwTon)
        {
            const double deltaTimeHour = 0.01;
            double totalTime = 0.0;
            double grossWeightTon = zfwTon + fuelCapacityTon;

            while (grossWeightTon > zfwTon)
            {
                double ff = FuelFlowTonPerHour(grossWeightTon);
                grossWeightTon -= ff * deltaTimeHour;
                totalTime += deltaTimeHour;
            }

            return totalTime * cruiseSpeedKnots;
        }

        private static double GetRangePara2(double zfwTon)
        {
            const double deltaTimeHour = 0.01;
            double totalTime = 0.0;
            double grossWeightTon = zfwTon + fuelCapacityTon;

            double actualUsableFuelTon = (fuelCapacityTon - reserveFuelTon) /
                (1.0 + contingencyPercent / 100.0);

            while (actualUsableFuelTon > 0.0)
            {
                double ff = FuelFlowTonPerHour(grossWeightTon);
                grossWeightTon -= ff * deltaTimeHour;
                actualUsableFuelTon -= ff * deltaTimeHour;
                totalTime += deltaTimeHour;
            }

            return totalTime * cruiseSpeedKnots;
        }

        private static double FuelFlowTonPerHour(double grossWeightTon)
        {
            return (17.271 * grossWeightTon + 114.62) * 2.0 / 1000.0;
        }
    }
}
