using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public class OutflowService : IOutflowService
    {
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "OutflowTransaction.json");
        private readonly AuthenticationStateService _authStateService;
        public OutflowService(AuthenticationStateService authStateService)
        {
            _authStateService = authStateService; 
        }
        public async Task<List<Outflow>> LoadOutflowAsync()
        {
            try
            {
                if (!File.Exists(transactionFilePath))
                {
                    return new List<Outflow>();
                }

                var json = await File.ReadAllTextAsync(transactionFilePath);
                return JsonSerializer.Deserialize<List<Outflow>>(json) ?? new List<Outflow>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Outflow>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Outflow>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading inflow: {ex.Message}");
                return new List<Outflow>();
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
        public async Task AddOutflowAsync(Outflow outflow)
        {

            try
            {
                outflow.user_id = GetLoggedInUserId();
                var transactions = await LoadOutflowAsync();
                outflow.outflowId = transactions.Count > 0 ? transactions.Max(t => t.outflowId) + 1 : 1;
                transactions.Add(outflow);
                await SaveOutflowAsync(transactions);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error adding task: {ex.Message}");
                throw;
            }
        }


        public async Task SaveOutflowAsync(List<Outflow> outflowDetails)
        {
            try
            {
                var json = JsonSerializer.Serialize(outflowDetails, new JsonSerializerOptions { WriteIndented = true });

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
