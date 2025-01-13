using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public interface IDebtService
    {
        Task SaveDebtAsync(List<Debt> debtDetails);
        Task<List<Debt>> LoadDebtAsync();
        Task AddDebtAsync(Debt debt);
     }
}
