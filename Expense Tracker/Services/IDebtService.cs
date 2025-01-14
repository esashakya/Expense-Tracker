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
        Task<List<Debt>> GetDebtsByUserIdAsync(int userId);
        Task ChangeDebtStatusAsync(int debtId, int id);
       
    }
}
