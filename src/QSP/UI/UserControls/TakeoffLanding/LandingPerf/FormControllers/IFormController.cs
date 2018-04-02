using System;
namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public interface IFormController
    {
        event EventHandler CalculationCompleted;
        FormOptions Options { get; }
        void Compute(object sender, EventArgs e);
    }
}
