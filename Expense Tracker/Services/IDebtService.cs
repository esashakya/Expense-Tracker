using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public interface IDebtService //Interface for DebtService
    {
        Task SaveDebtAsync(List<Debt> debtDetails); //save the debt details
        Task<List<Debt>> LoadDebtAsync(); //load the debt details
        Task AddDebtAsync(Debt debt); //add specific debt detail
        Task<List<Debt>> GetDebtsByUserIdAsync(int userId); //get debt details of specific user id
        Task ChangeDebtStatusAsync(int debtId, int id); //method to change the status of debt while updating the outflow
        Task<List<Debt>> GetDebtsByTitleAsync(string title, int userId); //method for filtering debt title


    }
}
