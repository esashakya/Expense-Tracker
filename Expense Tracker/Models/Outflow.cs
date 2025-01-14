using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class Outflow
    {
        public int outflowId { get; set; }
        public int user_id { get; set; }
        public decimal outflow_amount { get; set; }
        public DateTime outflow_date { get; set; }
        public string outflow_tag { get; set; }
        public string outflow_title { get; set; }
        public string outflow_label { get; set; }
        public string outflow_note { get; set; }
    }
}
