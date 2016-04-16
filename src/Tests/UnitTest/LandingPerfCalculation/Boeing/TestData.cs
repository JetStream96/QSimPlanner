namespace UnitTest.LandingPerfCalculation.Boeing
{
    public class TestData
    {
        public string AllText
        {
            get
            {
                return @"<LandingData>
		<Parameters>
        <Aircraft>B737-600</Aircraft>
        <Description></Description>
        <Designator>B736</Designator>
      
				<LengthUnit>M</LengthUnit>				
				<WeightRef>50000</WeightRef>
				<!-- All in KG -->

				<WeightStep>5000</WeightStep>
				<!-- All in KG -->

				<Brakes>
						<Dry>MAX MANUAL;MAX AUTO</Dry>
						<Wet>MAX MANUAL</Wet>
				</Brakes>

        <Reversers>Both;One Rev;No Rev</Reversers>
      
		</Parameters>
			
		<Data>
				<Flaps>30</Flaps>
				<Dry>750 55/-40 15/15 -30 95 5 -5 15 -15 55 10 25
955 60/-60 20/20 -35 125 0 0 20 -20 95 0 0</Dry>
				<Good>1200 85/-80 30/30 -55 205 30 -20 30 -25 100 65 145</Good>
				<Medium>1595 130/-120 50/50 -85 335 70 -50 45 -40 130 175 430</Medium>
				<Poor>2045 185/-170 65/65 -130 525 170 -105 55 -50 150 370 1030</Poor>
		</Data>
		
		<Data>
				<Flaps>40</Flaps>
				<Dry>750 55/-40 15/15 -30 100 10 -5 15 -15 60 10 25
930 55/-55 20/20 -35 125 0 0 20 -20 95 0 0</Dry>
				<Good>1185 85/-70 30/30 -55 205 30 -20 30 -25 100 65 140</Good>
				<Medium>1565 130/-120 45/45 -85 330 70 -50 45 -40 130 165 405</Medium>
				<Poor>2005 180/-165 60/60 -130 525 165 -105 55 -50 150 345 945</Poor>
		</Data>

</LandingData> ";
            }
        }
    }
}
