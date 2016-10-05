using QSP.Common;
using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class InputValidator
    {
        /// <summary>
        /// Parse the text of control to double and verify it's no smaller 
        /// than 0. 
        /// </summary>
        /// <exception cref="InvalidUserInputException" ></exception>
        public static double ParseNonNegative(
            Control control, string exceptionMsg)
        {
            double num;

            if (!double.TryParse(control.Text, out num) || num < 0.0)
            {
                throw new InvalidUserInputException(exceptionMsg);
            }

            return num;
        }
    }
}
