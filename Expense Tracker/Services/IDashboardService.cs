using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public interface IDashboardService
    {
        Task<decimal> CalculateTotalInflowAsync(int id);
        Task<decimal> CalculateTotalOutflowAsync(int id);
        Task<(decimal TotalDebt, decimal TotalPending, decimal TotalCleared)> CalculateTotalDebtAsync(int id);
        Task<List<Transaction>> ListTransactions(int id);

        Task<decimal> CalculateTotalAsync(int id);



    }
}
