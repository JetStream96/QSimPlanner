using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public static class Utilities
    {
        #region SplitSidStarTransition

        /// <summary>
        /// Gets a TerminalProcedureName containing the name of SID/STAR and transition.
        /// </summary>
        public static TerminalProcedureName SplitSidStarTransition(string sidStar)
        {
            string sidName = null;
            string transName = null;

            if (sidStar.IndexOf('.') != -1)
            {
                sidName = sidStar.Substring(0, sidStar.IndexOf('.'));
                transName = sidStar.Substring(sidStar.IndexOf('.') + 1);
            }
            else
            {
                sidName = sidStar;
                transName = "";
            }
            return new TerminalProcedureName(sidName, transName);
        }

        public class TerminalProcedureName
        {
            public string ProcedureName { get; set; }
            public string TransitionName { get; set; }

            public TerminalProcedureName(string ProcedureName,string TransitionName)
            {
                this.ProcedureName = ProcedureName;
                this.TransitionName = TransitionName;
            }
        }

        #endregion
    }
}
