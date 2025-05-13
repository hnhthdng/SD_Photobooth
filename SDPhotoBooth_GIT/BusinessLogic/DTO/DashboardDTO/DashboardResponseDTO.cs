using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.DashboardDTO
{
    public class TotalOrderStaticResponseDTO
    {
        public int TotalOrder { get; set; }
        public int TotalOrderPrev { get; set; }
    }

    public class TotalDepositStaticResponseDTO
    {
        public int TotalDeposit { get; set; }
        public int TotalDepositPrev { get; set; }
    }

    public class TotalProfitStaticResponseDTO
    {
        public decimal TotalProfit { get; set; }
        public decimal TotalProfitPrev { get; set; }
    }

    public class TotalUserStaticResponseDTO
    {
        public int TotalUser { get; set; }
        public int TotalUserPrev { get; set; }
    }

    public class TotalRevenueStaticResponseDTO
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenuePrev { get; set; }
    }

    public class TotalRevenueByPlatformTypeDTO
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Quarter { get; set; }
        public DateTime? Day { get; set; }

        public decimal TotalRevenueMobile { get; set; }
        public decimal TotalRevenueStore { get; set; }
    }

    public class RevenueByPlatformStatisticsResponseDTO
    {
        public GroupingType GroupingType { get; set; }
        public List<TotalRevenueByPlatformTypeDTO> Data { get; set; }
    }

    public class UsageChannelDTO
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Quarter { get; set; }
        public DateTime? Day { get; set; }

        public string Name { get; set; }
        public int TotalUsage { get; set; }
    }

    public class UsageChannelStatisticsResponseDTO
    {
        public GroupingType GroupingType { get; set; }
        public List<UsageChannelDTO> Data { get; set; }
    }

    public class RevenueStaffDTO
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Quarter { get; set; }
        public DateTime? Day { get; set; }

        public string? Name { get; set; }
        public string? Id { get; set; }
        public decimal TotalRevenue { get; set; }
        public int Count { get; set; }
    }

    public class RevenueStaffStatisticsResponseDTO
    {
        public GroupingType GroupingType { get; set; }
        public List<RevenueStaffDTO> Data { get; set; }
    }
}
