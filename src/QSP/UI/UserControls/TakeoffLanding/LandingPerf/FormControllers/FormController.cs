using System;
namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public interface IFormController
    {
        event EventHandler CalculationCompleted;

        void Initialize();
        void SurfCondChanged(object sender, EventArgs e);
        void WeightUnitChanged(object sender, EventArgs e);
        void FlapsChanged(object sender, EventArgs e);
        void ReverserChanged(object sender, EventArgs e);
        void BrakesChanged(object sender, EventArgs e);
        void Compute(object sender, EventArgs e);
    }
}
