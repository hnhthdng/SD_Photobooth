using BusinessLogic.DTO.DashboardDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Utils
{
    public static class TimeRangeHelper
    {
        public static (DateTime start, DateTime startPrev) GetTimeRange(StaticType staticType)
        {
            DateTime today = DateTime.Today;
            return staticType switch
            {
                StaticType.Day => (today, today.AddDays(-1)),
                StaticType.Week => (today.AddDays(-(int)today.DayOfWeek), today.AddDays(-(int)today.DayOfWeek - 7)),
                StaticType.Month => (new DateTime(today.Year, today.Month, 1), new DateTime(today.Year, today.Month, 1).AddMonths(-1)),
                StaticType.Year => (new DateTime(today.Year, 1, 1), new DateTime(today.Year, 1, 1).AddYears(-1)),
                _ => (today, today)
            };
        }

        public static TimeSpan GetStepSize(StaticType staticType)
        {
            DateTime today = DateTime.Today;
            return staticType switch
            {
                StaticType.Day => TimeSpan.FromDays(1),
                StaticType.Week => TimeSpan.FromDays(7),
                StaticType.Month => TimeSpan.FromDays(DateTime.DaysInMonth(today.Year, today.Month)),
                StaticType.Year => TimeSpan.FromDays(365),
                _ => TimeSpan.Zero
            };
        }

        public static TimeSpan GetStepSize(GroupingType staticType)
        {
            DateTime today = DateTime.Today;
            return staticType switch
            {
                GroupingType.Day => TimeSpan.FromDays(1),
                GroupingType.Month => TimeSpan.FromDays(DateTime.DaysInMonth(today.Year, today.Month)),
                GroupingType.Quarter => TimeSpan.FromDays(90),
                GroupingType.Year => TimeSpan.FromDays(365),
                _ => TimeSpan.Zero
            };
        }
    }
}
