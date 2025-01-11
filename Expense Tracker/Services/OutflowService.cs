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
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "Transaction.json");
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
