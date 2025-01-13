using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Expense_Tracker.Models;

namespace Expense_Tracker.Services
{
    public class InflowService: IInflowService
    {
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "InflowTransaction.json");
        private readonly AuthenticationStateService _authStateService; // using authenicationService to collect loggedin user id

        public InflowService(AuthenticationStateService authStateService)
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
        public async Task AddInflowAsync(Inflow inflow)
        {

            try
            {
                inflow.user_id = GetLoggedInUserId();
                var transactions = await LoadInflowAsync();
                inflow.inflowId = transactions.Count > 0 ? transactions.Max(t => t.inflowId) + 1 : 1;
                transactions.Add(inflow);
                await SaveInflowAsync(transactions);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error adding task: {ex.Message}");
                throw; 
            }
        }

        public async Task<List<Inflow>> GetInflowsByUserIdAsync(int userId)
        {
            try
            {
                var transactions = await LoadInflowAsync();
                
                return (transactions ?? new List<Inflow>()).Where(t => t.user_id == userId).ToList();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error retrieving tasks for user {userId}: {ex.Message}");
                return new List<Inflow>(); 
            }
        }
        public async Task<List<Inflow>> LoadInflowAsync()
        {
            try
            {
                if (!File.Exists(transactionFilePath))
                {
                    return new List<Inflow>();
                }

                var json = await File.ReadAllTextAsync(transactionFilePath);
                return JsonSerializer.Deserialize<List<Inflow>>(json) ?? new List<Inflow>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Inflow>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading transaction: {ioEx.Message}");
                return new List<Inflow>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading transaction: {ex.Message}");
                return new List<Inflow>();
            }
        }

        

        public async Task SaveInflowAsync(List<Inflow> inflowDetails)
        {
            try
            {
                var json = JsonSerializer.Serialize(inflowDetails, new JsonSerializerOptions { WriteIndented = true });

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
