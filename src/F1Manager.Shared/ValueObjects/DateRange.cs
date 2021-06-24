using System;

namespace F1Manager.Shared.ValueObjects
{
    public class DateRange
    {

        public DateTimeOffset From { get; set; }
        public DateTimeOffset? To { get; set; }

        public bool IsValid => !To.HasValue || From < To.Value;

        public DateRange() { }
        public DateRange(DateTimeOffset from, DateTimeOffset? to)
        {
            From = from;
            To = to;
        }
    }
}
