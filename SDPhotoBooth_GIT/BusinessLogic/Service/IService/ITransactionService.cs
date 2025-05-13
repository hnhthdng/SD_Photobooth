using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.TransactionDTO;
using DataAccess.Extensions.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service.IService
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionResponseDTO>> GetAllTransactions(PaginationParams? pagination);
        Task<TransactionResponseDTO> GetTransactionById(int Id);
        Task<IEnumerable<TransactionResponseDTO>> GetTransactionByCusId(string cusId);
        Task<TransactionResponseDTO> CreateTransaction(TransactionRequestDTO transactionRequestDTO);
        Task<TotalRevenueStaticResponseDTO> StaticRevenue(StaticType staticType);
        Task<RevenueByPlatformStatisticsResponseDTO> StaticRevenueByPlatformType(GroupingType staticType);
        Task<int> GetCount();
    }
}
