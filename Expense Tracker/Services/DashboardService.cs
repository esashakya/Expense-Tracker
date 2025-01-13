using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
using System.Text.Json;
namespace Expense_Tracker.Services
{
    public class DashboardService: IDashboardService
    {
        private readonly string DebtTransactionFilePath = Path.Combine(AppContext.BaseDirectory, "DebtTransaction.json");
        private readonly string InflowTransactionFilePath = Path.Combine(AppContext.BaseDirectory, "InflowTransaction.json");
        private readonly string OutflowTransactionFilePath = Path.Combine(AppContext.BaseDirectory, "OutflowTransaction.json");
        private readonly AuthenticationStateService _authStateService;
        public DashboardService(AuthenticationStateService authStateService)
        {
            _authStateService = authStateService;
        }
        private int GetLoggedInUserId()
        {
            var loggedUser = _authStateService.GetAuthenticatedUser(); //get user details
            if (loggedUser == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            return loggedUser.user_id; // user id is returned
        }
        public async Task<decimal> CalculateTotalInflowAsync()
        {
            try
            {
                if (!File.Exists(InflowTransactionFilePath))
                {
                    return 0;
                }
                int LoggedinUser = GetLoggedInUserId();
                var json = await File.ReadAllTextAsync(InflowTransactionFilePath);
                var inflows = JsonSerializer.Deserialize<List<Inflow>>(json) ?? new List<Inflow>();

               
                decimal TotalInflow = inflows.Where(InflowObject => InflowObject.user_id == LoggedinUser).Sum(InflowObject => (decimal)InflowObject.inflow_amount);
                return TotalInflow;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading inflow: {ex.Message}");
                return 0; 
            }
        }
        public async Task<decimal> CalculateTotalOutflowAsync()
        {
            try
            {
                if (!File.Exists(OutflowTransactionFilePath))
                {
                    return 0;
                }
                int LoggedinUser = GetLoggedInUserId();
                var json = await File.ReadAllTextAsync(OutflowTransactionFilePath);
                var outflows = JsonSerializer.Deserialize<List<Outflow>>(json) ?? new List<Outflow>();


                decimal TotalOutflow = outflows.Where(OutflowObject => OutflowObject.user_id == LoggedinUser).Sum(OutflowObject => (decimal)OutflowObject.outflow_amount);
                return TotalOutflow;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading inflow: {ex.Message}");
                return 0;
            }
        }
        public async Task<(decimal TotalDebt, decimal TotalPending, decimal TotalCleared)> CalculateTotalDebtAsync()
        {
            try
            {
                if (!File.Exists(DebtTransactionFilePath))
                {
                    return (0,0,0);
                }
                int LoggedinUser = GetLoggedInUserId();
                var json = await File.ReadAllTextAsync(DebtTransactionFilePath);
                var debts = JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();


                decimal TotalDebt = debts.Sum(DebtObject => (decimal)DebtObject.debt_amount);
                decimal TotalPending = debts.Where(DebtObject => !DebtObject.cleared && DebtObject.user_id == LoggedinUser).Sum(DebtObject => (decimal)DebtObject.debt_amount); //amount which is not cleared is calculated
                decimal TotalCleared = debts.Where(DebtObject => DebtObject.cleared && DebtObject.user_id == LoggedinUser).Sum(DebtObject => (decimal)DebtObject.debt_amount); //cleared debts
                return (TotalDebt, TotalPending, TotalCleared);
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return (0,0,0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading inflow: {ex.Message}");
                return (0,0,0);
            }
        }
    }

}
