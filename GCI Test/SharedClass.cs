using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCI_Test
{
    public static class SharedClass
    {
        public static List<TransactionList> ExistingTransacionList()
        {
            List<TransactionList> transactionList = new List<TransactionList>
            {
                new TransactionList
                    {
                        Date = "20230505",TransactionId = "20230505-01",Type = 'D',Amount = 100.00m,Account = "AC001",Balance = 100M
                    },
                new TransactionList
                    {
                        Date = "20230601",TransactionId = "20230601-01",Type = 'D',Amount = 150.00m,Account = "AC001",Balance = 250m
                    },
                new TransactionList
                    {
                        Date = "20230626",TransactionId = "20230626-01",Type = 'W',Amount = 20.00m,Account = "AC001",Balance = 230m
                    },
                new TransactionList
                    {
                        Date = "20230626",TransactionId = "20230626-02",Type = 'W',Amount = 100.00m,Account = "AC001",Balance = 130m
                    }
            };

            return transactionList;
        }

        public static List<InterestRule> InterestRulesList()
        {
            List<InterestRule> rulesList = new List<InterestRule>
            {
                new InterestRule
                    {
                        Date = "20230101",RuleId = "Rule01",Rate = 1.95m
                    },
                new InterestRule
                    {
                        Date = "20230520",RuleId = "Rule02",Rate = 1.90m
                    },
                 new InterestRule
                    {
                        Date = "20230615",RuleId = "Rule03",Rate = 2.20m
                    },
                 new InterestRule
                    {
                        Date = "19970101",RuleId = "Rule00",Rate = 0.0m, IsDefault = true //If the rule one starts from 20230202 and transaction is made on 20230101
                    }
            };

            return rulesList;
        }

    }
}
