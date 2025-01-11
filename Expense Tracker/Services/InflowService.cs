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
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "Transaction.json");
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
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Inflow>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading inflow: {ex.Message}");
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
