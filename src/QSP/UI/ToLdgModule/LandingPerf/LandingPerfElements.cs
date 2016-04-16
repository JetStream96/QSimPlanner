using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.LandingPerf
{
    public class LandingPerfElements
    {
        public Label AirportName { get; private set; }
        public TextBox Airport { get; private set; }
        public TextBox Length { get; private set; }
        public TextBox Elevation { get; private set; }
        public TextBox rwyHeading { get; private set; }
        public TextBox windDirection { get; private set; }
        public TextBox windSpeed { get; private set; }
        public TextBox oat { get; private set; }
        public TextBox pressure { get; private set; }
        public TextBox weight { get; private set; }
        public TextBox appSpeedIncrease { get; private set; }
        public ComboBox runway { get; private set; }
        public ComboBox lengthUnit { get; private set; }
        public ComboBox slope { get; private set; }
        public ComboBox tempUnit { get; private set; }
        public ComboBox brake { get; private set; }
        public ComboBox surfCond { get; private set; }
        public ComboBox pressureUnit { get; private set; }
        public ComboBox wtUnit { get; private set; }
        public ComboBox flaps { get; private set; }
        public ComboBox reverser { get; private set; }
        public RichTextBox result { get; private set; }

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
            this.rwyHeading = rwyHeading;
            this.windDirection = windDirection;
            this.windSpeed = windSpeed;
            this.oat = oat;
            this.pressure = pressure;
            this.weight = weight;
            this.appSpeedIncrease = appSpeedIncrease;
            this.runway = runway;
            this.lengthUnit = lengthUnit;
            this.slope = slope;
            this.tempUnit = tempUnit;
            this.brake = brake;
            this.surfCond = surfCond;
            this.pressureUnit = pressureUnit;
            this.wtUnit = wtUnit;
            this.flaps = flaps;
            this.reverser = reverser;
            this.result = result;
        }
    }
}
