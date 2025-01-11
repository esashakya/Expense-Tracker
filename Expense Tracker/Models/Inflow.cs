
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class Inflow
    {
        public string inflowId { get; set; }
        public string user_id { get; set; }
        public int inflow_amount {  get; set; } 
        public DateTime inflow_date { get; set; }
        public string inflow_tag {  get; set; }
        public string inflow_label { get; set; }
        public string inflow_note {  get; set; }

    }
}
