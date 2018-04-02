using System.Windows.Forms;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf
{
    public class TOPerfElements
    {
        public Label AirportName { get; }
        public TextBox Airport { get; }
        public TextBox Length { get; }
        public TextBox Elevation { get; }
        public TextBox RwyHeading { get; }
        public TextBox WindDirection { get; }
        public TextBox WindSpeed { get; }
        public TextBox Oat { get; }
        public TextBox Pressure { get; }
        public TextBox Weight { get; }
        public ComboBox lengthUnit { get; }
        public ComboBox Slope { get; }
        public ComboBox TempUnit { get; }
        public ComboBox SurfCond { get; }
        public ComboBox PressureUnit { get; }
        public ComboBox WtUnit { get; }
        public ComboBox Flaps { get; }
        public ComboBox ThrustRating { get; }
        public ComboBox AntiIce { get; }
        public ComboBox Packs { get; }
        public RichTextBox Result { get; }

        public TOPerfElements(
            Label AirportName,
            TextBox Airport,
            TextBox Length,
            TextBox Elevation,
            TextBox rwyHeading,
            TextBox windDirection,
            TextBox windSpeed,
            TextBox oat,
            TextBox pressure,
            TextBox weight,
            ComboBox lengthUnit,
            ComboBox slope,
            ComboBox tempUnit,
            ComboBox surfCond,
            ComboBox pressureUnit,
            ComboBox wtUnit,
            ComboBox flaps,
            ComboBox thrustRating,
            ComboBox AntiIce,
            ComboBox Packs,
            RichTextBox result)
        {
            this.AirportName = AirportName;
            this.Airport = Airport;
            this.Length = Length;
            this.Elevation = Elevation;
            this.RwyHeading = rwyHeading;
            this.WindDirection = windDirection;
            this.WindSpeed = windSpeed;
            this.Oat = oat;
            this.Pressure = pressure;
            this.Weight = weight;
            this.lengthUnit = lengthUnit;
            this.Slope = slope;
            this.TempUnit = tempUnit;
            this.SurfCond = surfCond;
            this.PressureUnit = pressureUnit;
            this.WtUnit = wtUnit;
            this.Flaps = flaps;
            this.ThrustRating = thrustRating;
            this.AntiIce = AntiIce;
            this.Packs = Packs;
            this.Result = result;
        }
    }
}
