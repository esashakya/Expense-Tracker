using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class Transaction
    {

        public string transaction_title { get; set; }
        public decimal transaction_amount { get; set; }
        public DateTime transaction_date { get; set; }
        public string transaction_tag { get; set; }
        public string transaction_label { get; set; }
        public string transaction_note { get; set; }
    }
}
