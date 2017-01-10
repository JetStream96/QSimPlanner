using System;
using QSP.MathTools;
using QSP.Utilities;

namespace QSP.UI.UserControls.TakeoffLanding.Common.AirportInfo
{
    // Keep a collection of doubles, representing the items inside
    // the comboBox. The increment is 0.1.
    //
    // Min/max values are set but it's possible to adjust for smaller/larger
    // slope values.
    //        
    public class SlopeComboBoxController
    {
        public double[] items { get; private set; }

        public SlopeComboBoxController(double min, double max)
        {
            SetItems(min, max);
        }

        public void SetItems(double min, double max)
        {
            ExceptionHelpers.Ensure<ArgumentException>(max >= min);

            int start = (int)Math.Round(min * 10);
            int end = (int)Math.Round(max * 10);

            items = new double[end - start + 1];

            for (int i = start; i <= end; i++)
            {
                items[i - start] = i * 0.1;
            }
        }

        public int NearestIndex(double slope)
        {
            int index = Numbers.RoundToInt((slope - items[0]) / 0.1);
            ExceptionHelpers.Ensure<ArgumentOutOfRangeException>(
                index >= 0 && index < items.Length);

            return index;
        }

        public bool ResizeRequired(double slope)
        {
            return slope < items[0] ||
                slope > items[items.Length - 1];
        }
    }
}
