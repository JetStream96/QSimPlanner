using System.Windows.Forms;

namespace QSP.UI.Forms.LandingPerf
{
    public class LandingPerfElements
    {
        private Label airportName;
        private TextBox airport;
        private TextBox length;
        private TextBox elevation;
        private TextBox rwyHeading;
        private TextBox windDirection;
        private TextBox windSpeed;
        private TextBox oat;
        private TextBox pressure;
        private TextBox weight;
        private TextBox appSpeedIncrease;
        private ComboBox runway;
        private ComboBox lengthUnit;
        private ComboBox slope;
        private ComboBox tempUnit;
        private ComboBox brake;
        private ComboBox surfCond;
        private ComboBox pressureUnit;
        private ComboBox wtUnit;
        private ComboBox flaps;
        private ComboBox reverser;
        private RichTextBox result;

        public LandingPerfElements(
            Label airportName,
            TextBox airport,
            TextBox length,
            TextBox elevation,
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
            this.airportName = airportName;
            this.airport = airport;
            this.length = length;
            this.elevation = elevation;
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
