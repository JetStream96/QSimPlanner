using QSP.UI.Controllers;
using QSP.UI.Presenters.FuelPlan;
using System;
using System.Collections.Generic;
using QSP.UI.Views.FuelPlan.Routes.Actions;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateRowView : ISupportActionContextMenu, ISelectedProcedureProvider
    {
        AlternateRowPresenter Presenter { get; }

        event EventHandler IcaoChanged;

        /// <summary>
        /// Can be "AUTO".
        /// </summary>
        IEnumerable<string> RunwayList { set; }

        new string Rwy { get; set; }
        new string Icao { get; set; }
    }
}
