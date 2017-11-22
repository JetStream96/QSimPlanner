﻿using QSP.UI.Controllers;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Views.Route.Actions;
using System;
using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateRowView : ISupportActionContextMenu, ISelectedProcedureProvider
    {
        AlternateRowPresenter Presenter { get; }

        event EventHandler IcaoChanged;

        /// <summary>
        /// Can be "AUTO", or "AUTO (10)" if the runway is automatically computed. 
        /// </summary>
        IEnumerable<string> RunwayList { set; }
    }
}
