using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Services
{
    public interface IDashboardService
    {
        Task<decimal> CalculateTotalInflowAsync(int id);
        Task<decimal> CalculateTotalOutflowAsync(int id);
        Task<(decimal TotalDebt, decimal TotalPending, decimal TotalCleared)> CalculateTotalDebtAsync(int id);
        

    }
}
