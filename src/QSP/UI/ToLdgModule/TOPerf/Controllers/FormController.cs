using QSP.TOPerfCalculation;
using System;

namespace QSP.UI.ToLdgModule.TOPerf.Controllers
{
    public class FormController
    {
        protected PerfTable acPerf;
        protected TOPerfElements elements;
        public event EventHandler CalculationCompleted;

        public FormController(PerfTable acPerf, TOPerfElements elements)
        {
            this.acPerf = acPerf;
            this.elements = elements;
        }

        /// <summary>
        /// Initialize the states of UI controls.
        /// </summary>
        public virtual void Initialize() { }
        
        public virtual void FlapsChanged(object sender,EventArgs e) { }

        public virtual void WeightUnitChanged(object sender, EventArgs e) { }
        
        public virtual void Compute(object sender, EventArgs e) { }

        protected virtual void OnCalculationComplete(EventArgs e)
        {
            CalculationCompleted?.Invoke(this, e);
        }
    }
}
