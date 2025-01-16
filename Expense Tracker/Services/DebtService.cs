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
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "DebtTransaction.json");//DebtTransaction.json will be used to store the details of debt
        private readonly AuthenticationStateService _authStateService;
        private readonly IOutflowService _outflowService; //using services
       
        private readonly IDashboardService _dashboardService;
        public DebtService(AuthenticationStateService authStateService,  IDashboardService dashboardService, IOutflowService outflowService)
        {
            _authStateService = authStateService;
            _outflowService = outflowService;
            _dashboardService = dashboardService;
        }
        public async Task<List<Debt>> LoadDebtAsync()//loading debt details
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
                Console.WriteLine($"JSON deserialization error while loading the JSON file");
                return new List<Debt>();
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Error encountered during calcukation");
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
        public async Task<List<Debt>> GetDebtsByUserIdAsync(int userId)//loading debt details according to the specified id
        {
            try
            {
                var transactions = await LoadDebtAsync();

                return (transactions ?? new List<Debt>()).Where(t => t.user_id == userId).ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while collecting the debts according to id");
                return new List<Debt>();
            }
        }
        public async Task<List<Debt>> GetDebtsByTitleAsync(string title,int userId)//filter debts according to title
        {
            try
            {
                var transactions = await LoadDebtAsync();

                return (transactions ?? new List<Debt>()).Where(t => t.user_id == userId && t.debt_title.ToLower() == title.ToLower()).ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while collecting the debts according to title");
                return new List<Debt>();
            }
        }
        public async Task ChangeDebtStatusAsync(int debtId, int id) //method to change the debt status after clearing the debts
        {
            try
            {
                var transactions = await LoadDebtAsync();

                var UpdatedDebt = transactions.Where(t => t.debtId == debtId).ToList(); //check the specific debt id that needs to be cleared
                int userId = id;
                var totalInflow = await _dashboardService.CalculateTotalInflowAsync(userId); //using the method from dashboard service to use its method for calculating the total inflow

                if (UpdatedDebt.Count != 0)
                {
                    foreach (var debt in UpdatedDebt)
                    {
                        if (totalInflow >= debt.debt_amount)
                        {
                            debt.debt_status = "Cleared";
                            debt.cleared = true;
                            totalInflow -= debt.debt_amount;
                            var outflow = new Outflow //after clearing debts,outflow details will be changed
                            {
                                user_id = debt.user_id,
                                outflow_title = debt.debt_title,
                                outflow_amount = debt.debt_amount,
                                outflow_date = debt.debt_date,
                                outflow_note = debt.debt_note,
                                outflow_label = "Outflow",
                                outflow_tag = debt.debt_tag,


                            };
                            await _outflowService.AddOutflowAsync(outflow); //using this method to update the outflow details
                            await _dashboardService.UpdateTotalInflowAsync(userId, debt.debt_amount);



                        }
                        else
                        {
                            Console.WriteLine($"Update in Debt status failed");
                        }
                        
                       
                    }
                    
                    


                    await SaveDebtAsync(transactions);
                    
                }
                else
                {
                    Console.WriteLine($"Update in Debt status failed");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error douns in collecting data");
                
            }
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


        public async Task SaveDebtAsync(List<Debt> debtDetails)//saving the debt details in json file
        {
            try
            {
                var json = JsonSerializer.Serialize(debtDetails, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(transactionFilePath, json);
            }
           
            catch (Exception ex)
            {
                Console.WriteLine($"Error found in inflow");
                throw;
            }
        }
    }
}
