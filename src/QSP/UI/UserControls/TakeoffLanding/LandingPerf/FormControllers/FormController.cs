using System;
using QSP.LandingPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class FormController
    {
        protected PerfTable acPerf;
        protected LandingPerfElements elements;
        public event EventHandler CalculationCompleted;

        public FormController(PerfTable acPerf, LandingPerfElements elements)
        {
            this.acPerf = acPerf;
            this.elements = elements;
        }

        /// <summary>
        /// Initialize the states of UI controls.
        /// </summary>
        public virtual void Initialize() { }

        public virtual void SurfCondChanged(object sender, EventArgs e) { }

        public virtual void WeightUnitChanged(object sender, EventArgs e) { }

        public virtual void FlapsChanged(object sender, EventArgs e) { }

        public virtual void ReverserChanged(object sender, EventArgs e) { }

        public virtual void BrakesChanged(object sender, EventArgs e) { }

        public virtual void Compute(object sender, EventArgs e) { }

        protected virtual void OnCalculationComplete(EventArgs e)
        {
            CalculationCompleted?.Invoke(this, e);
        }
    }
}
