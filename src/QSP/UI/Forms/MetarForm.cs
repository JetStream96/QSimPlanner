using QSP.Metar;
using QSP.Utilities;
using System;

namespace QSP.UI.Forms
{
    public partial class MetarForm
    {
        public string Metar { get; private set; }
        public string Taf { get; private set; }

        public MetarForm()
        {
            InitializeComponent();
        }
    }
}
