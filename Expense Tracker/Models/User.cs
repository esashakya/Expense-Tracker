using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class User
    {
        public string user_id { get; set; }

        public string username {  get; set; }
        public string password { get; set; }
        public string currencyType { get; set; }
        public string user_email { get; set; }
        public int total_amount { get; set; }
    }
}
