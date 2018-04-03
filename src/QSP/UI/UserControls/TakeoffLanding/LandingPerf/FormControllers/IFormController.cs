using QSP.LandingPerfCalculation;
namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public interface IFormController
    {
        FormOptions Options { get; }

        /// <summary>
        /// Get landing report.
        /// </summary>
        /// <exception cref="RunwayTooShortException"></exception>
        /// <exception cref="Exception"></exception>
        LandingReport GetReport(LandingParameters p);
    }
}
