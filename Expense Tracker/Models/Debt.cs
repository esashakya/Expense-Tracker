using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class Debt
    {
        public int debtId { get; set; }
        public int user_id { get; set; }
        public decimal debt_amount { get; set; }
        public DateTime debt_date { get; set; }
        public string debt_tag { get; set; }
        public string debt_label { get; set; }
        public string debt_note { get; set; }
        public DateTime debt_duedate { get; set; }
        public string debt_status { get; set; }
        public string debt_source { get; set; }
        public bool cleared { get; set; }


    }
}
