using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.DashboardDTO
{
    public class DashboardRequestStaticTotalDTO
    {
        public StaticType StaticType { get; set; }
    }

    public enum GroupingType
    {
        Day,
        Month,
        Quarter,
        Year
    }

    public enum ChannelGroupingType
    {
        Location,
        Booth
    }

    public enum StaticType
    {
        Day,
        Week,
        Month,
        Year
    }

    public enum TransactionType
    {
        All,
        Deposit,
        Order
    }

    public class DashboardRequestDTO
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class UsageChannelFilterDTO
    {
        public GroupingType StaticType { get; set; } = GroupingType.Day;
        public ChannelGroupingType ChannelGroupingType { get; set; } = ChannelGroupingType.Location;
        public int? LocationId { get; set; }
    }

    public class RevenueFilterDTO
    {
        public GroupingType StaticType { get; set; } = GroupingType.Day;
        public string? StaffId { get; set; }
    }
}
