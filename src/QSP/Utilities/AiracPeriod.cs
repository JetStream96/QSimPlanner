using System;

namespace QSP.Utilities
{
    public class AiracPeriod
    {
        public string Cycle { get; }
        public string PeriodText { get; }
        public TimePeriod Period { get; }

        public AiracPeriod(string Cycle, string PeriodText, TimePeriod Period)
        {
            this.Cycle = Cycle;
            this.PeriodText = PeriodText;
            this.Period = Period;
        }

        public bool IsWithinValidPeriod
        {
            get
            {
                var dateNow = DateTime.UtcNow.Date;
                return Period.Start <= dateNow && dateNow <= Period.End;
            }
        }
    }

    public struct TimePeriod
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public TimePeriod(DateTime Start, DateTime End)
        {
            this.Start = Start;
            this.End = End;
        }
    }

}