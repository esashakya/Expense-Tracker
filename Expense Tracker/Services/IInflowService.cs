using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public interface IInflowService
    {

        Task SaveInflowAsync(List<Inflow> inflowDetails);
        Task<List<Inflow>> LoadInflowAsync();
        Task AddInflowAsync(Inflow inflow);
        Task<List<Inflow>> GetInflowsByUserIdAsync(int userId);
    }
}
