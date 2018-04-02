using System;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public interface IFormController
    {
        event EventHandler CalculationCompleted;
        
        void Compute(object sender, EventArgs e);

        // Whenever user selects a different flaps/surface condition, etc,
        // we update all Combobox options. This is because, for example,
        // some thrust ratings are available only when certain flaps are in use
        // due to limit of available data.
        FormOptions Options { get; }
    }
}