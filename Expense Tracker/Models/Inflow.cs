
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class Inflow
    {
        public int inflowId { get; set; }
        public int user_id { get; set; }
        public decimal inflow_amount {  get; set; } 
        public DateTime inflow_date { get; set; }
        public string inflow_tag {  get; set; }
        public string custom_inflow_tag { get; set; }
        public string inflow_label { get; set; }
        public string inflow_note {  get; set; }

    }
}
