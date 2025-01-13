using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Services
{
    public interface IDashboardService
    {
        Task<decimal> CalculateTotalInflowAsync();
        Task<decimal> CalculateTotalOutflowAsync();
        Task<(decimal TotalDebt, decimal TotalPending, decimal TotalCleared)> CalculateTotalDebtAsync();

    }
}
