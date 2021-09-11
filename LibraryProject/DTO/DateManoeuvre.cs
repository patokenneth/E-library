using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.DTO
{
    public static class DateManoeuvre
    {
        public static DateTime Businessdays(this DateTime date, int defaultdays)
        {
            while (defaultdays > 0)
            {
                date = date.AddDays(1);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    defaultdays -= 1;
                }
            }

            return date;
        }

        public static int daydifference(DateTime defaultdate, DateTime currentdate)
        {
            int counter = 0;

            int days = (currentdate - defaultdate).Days;
            for (int i = 0; i < days; i++)
            {
                if (defaultdate.AddDays(i + 1).DayOfWeek != DayOfWeek.Saturday && defaultdate.AddDays(i + 1).DayOfWeek != DayOfWeek.Sunday)
                {
                    counter = counter + 1;
                }
            }

            return counter;
        }
    }
}
