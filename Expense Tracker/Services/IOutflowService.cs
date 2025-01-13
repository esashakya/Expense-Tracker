using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public interface IOutflowService
    {
        Task SaveOutflowAsync(List<Outflow> outflowDetails);
        Task AddOutflowAsync(Outflow outflow);
        Task<List<Outflow>> LoadOutflowAsync();
        Task<List<Outflow>> GetOutflowsByUserIdAsync(int userId);
    }
}
