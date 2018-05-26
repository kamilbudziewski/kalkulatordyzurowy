using System;
using Nager.Date;

namespace payment
{
    class Dyzur
    {
        public Dyzur(DateTime startTime)
        {
            StartTime = startTime;
        }

        public DateTime StartTime { get; set; }

        public double _100Hours()
        {
            if (StartTime.DayOfWeek == DayOfWeek.Sunday || IsHoliday())
            {
                if (NextIsHoliday())
                {
                    return 24;
                }
                return 22;
            }

            if (StartTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return 17 + ((double) 35 / (double) 60);
            }

            if (NextIsHoliday()) return 10;
            return 8;
        }


        public double _50Hours()
        {
            if (StartTime.DayOfWeek == DayOfWeek.Sunday || IsHoliday())
            {
                if (NextIsHoliday()) return 0;
                return 2;
            }
            if (StartTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return 6.4;
            }

            if (NextIsHoliday()) return 6.5-((double)5/60);
            return 8.5-((double)5/60);
        }

        public double _20Hours()
        {
            return 8;
        }
        
        public double _ZejsciaHours()
        {
            if (StartTime.DayOfWeek == DayOfWeek.Friday || StartTime.DayOfWeek == DayOfWeek.Saturday)
                return 0;
            if (NextIsHoliday())
            {
                return 0;
            }

            return 7 + ((double) 35 / (double) 60);
        }

        public double _0Hours()
        {
            if (IsHoliday() || StartTime.DayOfWeek == DayOfWeek.Saturday || StartTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return 24;
            }

            return 16 + ((double) 25 / 60);
        }
        
        private bool IsHoliday()
        {
            return DateSystem.IsPublicHoliday(StartTime, CountryCode.PL);
        }

        private bool NextIsHoliday()
        {
            return DateSystem.IsPublicHoliday(StartTime.AddDays(1), CountryCode.PL);
        }
    }
}