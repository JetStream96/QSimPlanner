using System;
using System.Windows.Forms;
using QSP.TOPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public class FormController
    {
        protected Control parentControl;
        protected PerfTable acPerf;
        protected TOPerfElements elements;
        public event EventHandler CalculationCompleted;

        public FormController(PerfTable acPerf, TOPerfElements elements, Control parentControl)
        {
            this.acPerf = acPerf;
            this.elements = elements;
            this.parentControl = parentControl;
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
