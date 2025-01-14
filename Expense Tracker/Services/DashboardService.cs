using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker.Models;
using System.Text.Json;
namespace Expense_Tracker.Services
{
    public class DashboardService : IDashboardService
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
        public async Task<decimal> CalculateTotalInflowAsync(int id)
        {
            try
            {
                if (!File.Exists(InflowTransactionFilePath))
                {
                    return 0;
                }

                var json = await File.ReadAllTextAsync(InflowTransactionFilePath);
                var inflows = JsonSerializer.Deserialize<List<Inflow>>(json) ?? new List<Inflow>();


                decimal TotalInflow = inflows.Where(InflowObject => InflowObject.user_id == id).Sum(InflowObject => (decimal)InflowObject.inflow_amount);
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

        public async Task<decimal> CalculateTotalOutflowAsync(int id)
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


                decimal TotalOutflow = outflows.Where(OutflowObject => OutflowObject.user_id == id).Sum(OutflowObject => (decimal)OutflowObject.outflow_amount);
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
        public async  Task<decimal> CalculateTotalAsync(int id)
        {
            try
            {
                if (!File.Exists(OutflowTransactionFilePath))
                {
                    return 0;
                }
                int LoggedinUser = GetLoggedInUserId();
                decimal totalOutflowAmount = await CalculateTotalOutflowAsync(LoggedinUser);
                decimal totalInflowAmount = await CalculateTotalInflowAsync(LoggedinUser);
                 (decimal TotalDebt, decimal TotalPending, decimal TotalCleared) = await CalculateTotalDebtAsync(LoggedinUser);
                decimal TotalDebtAmount = TotalPending;
                decimal TotalAmount = totalInflowAmount + TotalDebtAmount - totalOutflowAmount;
                return TotalAmount;
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
        public async Task<(decimal TotalDebt, decimal TotalPending, decimal TotalCleared)> CalculateTotalDebtAsync(int id)
        {
            try
            {
                if (!File.Exists(DebtTransactionFilePath))
                {
                    return (0, 0, 0);
                }
                int LoggedinUser = GetLoggedInUserId();
                var json = await File.ReadAllTextAsync(DebtTransactionFilePath);
                var debts = JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();


                decimal TotalDebt = debts.Where(DebtObject => !DebtObject.cleared && DebtObject.user_id == id).Sum(DebtObject => (decimal)DebtObject.debt_amount);
                decimal TotalPending = debts.Where(DebtObject => !DebtObject.cleared && DebtObject.user_id == id).Sum(DebtObject => (decimal)DebtObject.debt_amount); //amount which is not cleared is calculated
                decimal TotalCleared = debts.Where(DebtObject => DebtObject.cleared && DebtObject.user_id == id).Sum(DebtObject => (decimal)DebtObject.debt_amount); //cleared debts
                return (TotalDebt, TotalPending, TotalCleared);
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return (0, 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading inflow: {ex.Message}");
                return (0, 0, 0);
            }
        }
        public async Task<List<Transaction>> ListTransactions(int id)
        {
            var DebtJson = await File.ReadAllTextAsync(DebtTransactionFilePath);
            var debts = JsonSerializer.Deserialize<List<Debt>>(DebtJson) ?? new List<Debt>();
            var InflowJson = await File.ReadAllTextAsync(InflowTransactionFilePath);
            var inflows = JsonSerializer.Deserialize<List<Inflow>>(InflowJson) ?? new List<Inflow>();
            var OutflowJson = await File.ReadAllTextAsync(OutflowTransactionFilePath);
            var outflows = JsonSerializer.Deserialize<List<Outflow>>(OutflowJson) ?? new List<Outflow>();
            List<Transaction> transactions = new List<Transaction>();
            transactions.AddRange(debts.Where(debt => debt.user_id == id).Select(debt => new Transaction
            {
                transaction_amount = debt.debt_amount,
                transaction_date = debt.debt_date,
                transaction_label = debt.debt_label,
                transaction_tag = debt.debt_tag,
                transaction_note = debt.debt_note,
                transaction_title = debt.debt_title,
            }));
            transactions.AddRange(inflows.Where(inflow => inflow.user_id == id).Select(inflow => new Transaction
            {
                transaction_amount = inflow.inflow_amount,
                transaction_date = inflow.inflow_date,
                transaction_label = inflow.inflow_label,
                transaction_tag = inflow.inflow_tag,
                transaction_note = inflow.inflow_note,
                transaction_title=inflow.inflow_title,
            }));
            transactions.AddRange(outflows.Where(outflow => outflow.user_id == id).Select(outflow => new Transaction
            {
                transaction_amount = outflow.outflow_amount,
                transaction_date = outflow.outflow_date,
                transaction_label = outflow.outflow_label,
                transaction_tag = outflow.outflow_tag,
                transaction_note = outflow.outflow_note,
                transaction_title = outflow.outflow_title,
            }));
            return transactions;
        }
        
    }

}
