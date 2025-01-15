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
        Task SaveOutflowAsync(List<Outflow> outflowDetails); //save the outflow details
        Task AddOutflowAsync(Outflow outflow); //add specific outflow detail
        Task<List<Outflow>> LoadOutflowAsync(); //load the outflow detail
        Task<List<Outflow>> GetOutflowsByUserIdAsync(int userId); //get outflow details of specific user
        Task<List<Outflow>> GetOutflowsByTitleAsync(string title, int userId); //get outflow details of specific title


    }
}
