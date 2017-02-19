using System;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public interface IFormController
    {
        event EventHandler CalculationCompleted;

        void Initialize();
        void FlapsChanged(object sender, EventArgs e);
        void WeightUnitChanged(object sender, EventArgs e);
        void Compute(object sender, EventArgs e);
    }
}