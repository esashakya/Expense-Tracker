using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public class DebtService: IDebtService
    {
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "DebtTransaction.json");
        private readonly AuthenticationStateService _authStateService;
        public DebtService(AuthenticationStateService authStateService)
        {
            _authStateService = authStateService; 
        }
        public async Task<List<Debt>> LoadDebtAsync()
        {
            try
            {
                if (!File.Exists(transactionFilePath))
                {
                    return new List<Debt>();
                }

                var json = await File.ReadAllTextAsync(transactionFilePath);
                return JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Debt>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Debt>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading inflow: {ex.Message}");
                return new List<Debt>();
            }
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
        public async Task AddDebtAsync(Debt debt)
        {

            try
            {
                debt.user_id = GetLoggedInUserId();
                var transactions = await LoadDebtAsync();
                debt.debtId = transactions.Count > 0 ? transactions.Max(t => t.debtId) + 1 : 1;
                transactions.Add(debt);
                await SaveDebtAsync(transactions);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error adding task: {ex.Message}");
                throw;
            }
        }


        public async Task SaveDebtAsync(List<Debt> debtDetails)
        {
            try
            {
                var json = JsonSerializer.Serialize(debtDetails, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(transactionFilePath, json);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading inflow: {ioEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while saving inflow: {ex.Message}");
                throw;
            }
        }
    }
}
