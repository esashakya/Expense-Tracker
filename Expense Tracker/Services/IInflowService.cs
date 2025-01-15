using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public interface IInflowService//Interface for InflowService
    {

        Task SaveInflowAsync(List<Inflow> inflowDetails); // save inflow information methods
        Task<List<Inflow>> LoadInflowAsync(); //load inflow details
        Task AddInflowAsync(Inflow inflow); //add new inflow
        Task<List<Inflow>> GetInflowsByUserIdAsync(int userId); //getting specific inflow from specified user id
        Task<List<Inflow>> GetInflowsByTitleAsync(string title, int userId); //getting inflow details whose title is matched
    }
}
