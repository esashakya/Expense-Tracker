using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public interface IDashboardService //Interface for DashboardService
    {
        Task<decimal> CalculateTotalInflowAsync(int id); //method to calculate total inflow of loggedin user
        Task<decimal> CalculateTotalOutflowAsync(int id); //method to calculate total outflow of loggedin user
        Task<(decimal TotalDebt, decimal TotalPending, decimal TotalCleared)> CalculateTotalDebtAsync(int id); //method to calculate total debt of loggedin user
        Task<List<Transaction>> ListTransactions(int id); //list all the transactions made by user

        Task<decimal> CalculateTotalAsync(int id); //method to calculate the total amount of user
        Task<decimal> UpdateTotalInflowAsync(int id, decimal transactionAmt); //to update the inflow



    }
}
