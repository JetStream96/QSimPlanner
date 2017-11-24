using System.Collections.Generic;

namespace QSP.UI.Models.FuelPlan
{
    public class SelectedProcedures
    {
        public bool IsNone { get; }
        public bool IsAuto { get; }
        public IReadOnlyList<string> Strings { get; }

        private SelectedProcedures(bool IsNone, bool IsAuto, IReadOnlyList<string> Strings)
        {
            this.IsNone = IsNone;
            this.IsAuto = IsAuto;
            this.Strings = Strings;
        }

        public static SelectedProcedures None => new SelectedProcedures(true, false, null);

        public static SelectedProcedures Selected(string s) 
            => new SelectedProcedures(false, false, new[] { s });

        public static SelectedProcedures Auto(IReadOnlyList<string> x)
            => new SelectedProcedures(false,true, x);
    }
}
