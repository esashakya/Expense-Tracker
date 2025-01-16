using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Expense_Tracker.Models;
namespace Expense_Tracker.Services
{
    public class UserService:IUserService
    {
        private readonly string usersFilePath = Path.Combine(AppContext.BaseDirectory, "UserRecord.json"); //UserRecord will be used for storing user details

        public async Task SaveUserAsync(User user)//create new user
        {
            try
            {
                var users = await LoadUsersAsync();
                user.user_id = users.Count > 0 ? users.Max(t => t.user_id) + 1 : 1; //providing the value for id

                user.password = HashPassword(user.password);//hashing the user password

                users.Add(user); //adding the new user 
                await SaveUsersAsync(users); //saving new user details using this method
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving the new users");
                throw;
            }
        }

        public async Task<List<User>> LoadUsersAsync()//loading all the user details
        {
            try
            {
                if (!File.Exists(usersFilePath))
                {
                    return new List<User>();
                }

                var json = await File.ReadAllTextAsync(usersFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();//need to deserialize in order to calculate
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Deserialixation error in specific file");
                return new List<User>();
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during loading users process");
                return new List<User>();
            }
        }

        private async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(usersFilePath, json);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Error in saving new user and user information");
                throw;
            }
        }

       
        private string HashPassword(string password)//using this method to hash the password
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
