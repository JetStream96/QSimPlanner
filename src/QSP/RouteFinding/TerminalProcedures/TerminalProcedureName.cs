namespace QSP.RouteFinding.TerminalProcedures
{
    public class TerminalProcedureName
    {
        public string ProcedureName { get; private set; }
        public string TransitionName { get; private set; }

        public TerminalProcedureName(
            string ProcedureName, string TransitionName)
        {
            this.ProcedureName = ProcedureName;
            this.TransitionName = TransitionName;
        }

        /// <summary>
        /// Example for fullName: RIIVR2.HEC 
        /// </summary>
        public TerminalProcedureName(string fullName)
        {
            int index = fullName.IndexOf('.');

            if (index == -1)
            {
                ProcedureName = fullName;
                TransitionName = "";
            }
            else
            {
                ProcedureName = fullName.Substring(0, index);
                TransitionName = fullName.Substring(index + 1);
            }
        }
    }
}
