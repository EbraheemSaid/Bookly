using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Domain.Bookings
{
    public record DateRange
    {
        private DateRange()
        {

        }

        public DateOnly Start { get; init; }
        public DateOnly End { get; init; }
        public int LengthInDays => (End.DayNumber - Start.DayNumber);

        public static DateRange Create(DateOnly start, DateOnly end)
        {
            if (end < start)
                throw new ArgumentException("End date must be greater than or equal to start date.");
            return new DateRange
            {
                Start = start,
                End = end
            };
        }
    }

}
