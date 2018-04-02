using System.Windows.Forms;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf
{
    public class LandingPerfElements
    {
        public Label AirportName { get; private set; }
        public TextBox Airport { get; private set; }
        public TextBox Length { get; private set; }
        public TextBox Elevation { get; private set; }
        public TextBox RwyHeading { get; private set; }
        public TextBox WindDirection { get; private set; }
        public TextBox WindSpeed { get; private set; }
        public TextBox Oat { get; private set; }
        public TextBox Pressure { get; private set; }
        public TextBox Weight { get; private set; }
        public TextBox AppSpeedIncrease { get; private set; }
        public ComboBox Runway { get; private set; }
        public ComboBox LengthUnit { get; private set; }
        public ComboBox Slope { get; private set; }
        public ComboBox TempUnit { get; private set; }
        public ComboBox Brake { get; private set; }
        public ComboBox SurfCond { get; private set; }
        public ComboBox PressureUnit { get; private set; }
        public ComboBox WtUnit { get; private set; }
        public ComboBox Flaps { get; private set; }
        public ComboBox Reverser { get; private set; }
        public RichTextBox Result { get; private set; }

        public LandingPerfElements(
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
            TextBox appSpeedIncrease,
            ComboBox runway,
            ComboBox lengthUnit,
            ComboBox slope,
            ComboBox tempUnit,
            ComboBox brake,
            ComboBox surfCond,
            ComboBox pressureUnit,
            ComboBox wtUnit,
            ComboBox flaps,
            ComboBox reverser,
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
            this.AppSpeedIncrease = appSpeedIncrease;
            this.Runway = runway;
            this.LengthUnit = lengthUnit;
            this.Slope = slope;
            this.TempUnit = tempUnit;
            this.Brake = brake;
            this.SurfCond = surfCond;
            this.PressureUnit = pressureUnit;
            this.WtUnit = wtUnit;
            this.Flaps = flaps;
            this.Reverser = reverser;
            this.Result = result;
        }
    }
}
