using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCI_Test
{
    public class TransactionList
    {
        public string Date { get; set; }
        public string Account { get; set; }
        public char Type { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public decimal Balance { get; set; }
    }

    public class InterestRule
    {
        public string Date { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }
        public bool IsDefault { get; set; }
    }
}
