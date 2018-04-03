using QSP.TOPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public interface IFormController
    {
        // Whenever user selects a different flaps/surface condition, etc,
        // we update all Combobox options. This is because, for example,
        // some thrust ratings are available only when certain flaps are in use
        // due to limit of available data.
        FormOptions Options { get; }

        /// <summary>
        /// Get the takeoff report.
        /// </summary>
        /// <exception cref="RunwayTooShortException"></exception>
        /// <exception cref="PoorClimbPerformanceException"></exception>
        /// <exception cref="Exception"></exception>
        TOReport GetReport(TOParameters p, double tempIncrementCelsius);
    }
}
