using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class Outflow
    {
        public string outflowId { get; set; }
        public string user_id { get; set; }
        public int outflow_amount { get; set; }
        public DateTime outflow_date { get; set; }
        public string outflow_tag { get; set; }
        public string outflow_label { get; set; }
        public string outflow_note { get; set; }
    }
}
