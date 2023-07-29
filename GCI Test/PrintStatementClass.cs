using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCI_Test
{
    public static class PrintStatementClass
    {

        public static void PrintStatement()
        {
            Console.WriteLine();
            Console.WriteLine("Please enter account and month to generate the statement <Account>|<Month> (or enter blank to go back to the main menu):");
            Console.Write("> ");
            string input = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            string[] parts = input.Split('|');

            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                PrintStatement();
                return;
            }

            string account = parts[0].Trim();
            string month = Convert.ToInt32(parts[1].Trim()).ToString("D2");

            var transactions = ExistingTransacionList();

            if (Convert.ToInt32(month) > 12 || Convert.ToInt32(month) < 1)
            {
                Console.WriteLine("Invalid Month . Please try again.");
                PrintStatement();
            }

            var accountTransactions = transactions.Where(t => t.Account == account && t.Date.Substring(4, 2).StartsWith(month)).OrderBy(t => t.Date).ToList();

            if (accountTransactions.Count == 0)
            {
                Console.WriteLine("No Transaction on this month");
                return;
            }

            Console.WriteLine($"Account: {account}");
            Console.WriteLine("Date     | Txn Id      | Type | Amount | Balance |");

            foreach (var transaction in accountTransactions)
            {
                Console.WriteLine($"{transaction.Date} | {transaction.TransactionId} | {transaction.Type}    | {transaction.Amount,6:0.00} | {transaction.Balance,7:0.00} |");
            }

            decimal currentBalance = accountTransactions.Last().Balance; // Balance in the account in the first day of the month

            // Calculate and display interest for the month
            decimal totalInterest = GetInterest(month, accountTransactions);
            currentBalance += totalInterest;

            string lastDayOfMonth = GetLastDayOfMonth(month);
            string interestTxnId = $"{lastDayOfMonth}-I";
            Console.WriteLine($"{lastDayOfMonth} |             | I    |{totalInterest,6:0.00}  | {currentBalance,7:0.00} |");
        }

        static string GetLastDayOfMonth(string month)
        {
            int year = 2023; //User can enter only month so default value is taken as 2023
            int monthValue = int.Parse(month);
            int lastDay = DateTime.DaysInMonth(year, monthValue);
            return $"{year}{monthValue.ToString("D2")}{lastDay}";
        }

        static List<TransactionList> ExistingTransacionList()
        {
            List<TransactionList> transactionList = new List<TransactionList>
            {
                new TransactionList
                    {
                        Date = "20230505",TransactionId = "20230505-01",Type = 'D',Amount = 100.00m,Account = "AC001",Balance = 100m
                    },
                new TransactionList
                    {
                        Date = "20230601",TransactionId = "20230601-01",Type = 'D',Amount = 150.00m,Account = "AC001", Balance = 250m
                    },
                new TransactionList
                    {
                        Date = "20230626",TransactionId = "20230626-01",Type = 'W',Amount = 20.00m,Account = "AC001", Balance = 230m
                    },
                new TransactionList
                    {
                        Date = "20230626",TransactionId = "20230626-02",Type = 'W',Amount = 100.00m,Account = "AC001" , Balance = 130m
                    }
            };

            return transactionList;
        }

        static List<InterestRule> InterestRulesList()
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

        static InterestRule? GetInterestRule(DateTime date)
        {
            //Get the rule of the interest at the transaction date

            var interestrules = InterestRulesList();

            var interestruleCuurent = interestrules
                .OrderByDescending(r => Int32.Parse(r.Date))
                .FirstOrDefault(r => date > DateTime.ParseExact(r.Date, "yyyyMMdd", null)); 

            return interestruleCuurent;
        }

        static InterestRule? GetNextInterestRule(DateTime date)
        {

            //Get the Interest rule after the rule of the current transaction date
            var interestrules = InterestRulesList();

            var interestruleCuurent = interestrules
                .OrderBy(r => Int32.Parse(r.Date))
                .FirstOrDefault(r => date <= DateTime.ParseExact(r.Date, "yyyyMMdd", null));

            return interestruleCuurent;
        }

        static decimal GetInterest(string month, List<TransactionList> transactions)
        {
            var interestRules = InterestRulesList();
            decimal interestValue = 0;
            DateTime startDate = new DateTime(2023, Int32.Parse(month), 1);

            //If multiple transaction are made in same day then the last transaton with balance is taken
            var filteredTransactions = transactions
            .GroupBy(t => t.Date)
            .SelectMany(group => group.OrderByDescending(t => t.TransactionId).Take(1))
            .ToList();

            //If the transaction is made after the 1st day of the month
            //To Calculate the interest from first of the month to the day of transaction

            if (DateTime.ParseExact(filteredTransactions[0].Date, "yyyyMMdd", null) != startDate)
            {
                TransactionList firstDayTransaction = new TransactionList
                {
                    Date = startDate.Year.ToString("D2") + startDate.Month.ToString("D2") + startDate.Day.ToString("D2"),
                    TransactionId = startDate.Year.ToString() + startDate.Month.ToString("D2") + startDate.Day.ToString("D2") + "-XX",
                    
                    //balance in the 1st day of the month
                    Balance = filteredTransactions[0].Type == 'D' ?
                              filteredTransactions[0].Balance - filteredTransactions[0].Amount :
                              filteredTransactions[0].Balance + filteredTransactions[0].Amount,
                };
                filteredTransactions.Insert(0, firstDayTransaction);
            }

            for (int i = 0; i < filteredTransactions.Count; i++)
            {
                List<TransactionList> twoBreakTransactionList = new List<TransactionList>();

                twoBreakTransactionList.Add(filteredTransactions[i]);
                if (i != filteredTransactions.Count - 1) // the next transacion fro the next month is nul
                {
                    twoBreakTransactionList.Add(filteredTransactions[i + 1]);
                }


                interestValue += GetSpeicificRateValueBetweenTwoDate(month, twoBreakTransactionList);
            }

            return interestValue / 365;
        }

        static decimal GetSpeicificRateValueBetweenTwoDate(string month, List<TransactionList> filteredTransactions)
        {

            decimal totalInterestValue = 0;
            int days = 0;
            var interestRules = InterestRulesList();
            var firstRule = GetInterestRule(DateTime.ParseExact(filteredTransactions[0].Date, "yyyyMMdd", null));
            var firsRuleValid = GetNextInterestRule(DateTime.ParseExact(filteredTransactions[0].Date, "yyyyMMdd", null));
            var secondRule = filteredTransactions.Count == 1 ? null :
               GetInterestRule(DateTime.ParseExact(filteredTransactions[1].Date, "yyyyMMdd", null)); //if the transaction is the last transaction of the month

            string currentBreakDate = filteredTransactions[0].Date;

            var totalRulesInMonth = interestRules.Where(x => x.Date.Substring(4, 2) == month && x.IsDefault == false
            && (DateTime.ParseExact(filteredTransactions[0].Date, "yyyyMMdd", null) < (DateTime.ParseExact(x.Date, "yyyyMMdd", null)))).ToList();
            var totalRulesCount = totalRulesInMonth.Count();

            if (secondRule == null)
            {
                //Incase of multiple rule in one month but only one transaction is made in the month.

                for (int i = 0; i <= totalRulesCount; i++)
                {

                    string nextBreakDate = (i == totalRulesCount) 
                        ? GetLastDayOfMonth(month) : totalRulesInMonth[i].Date;

                    var nextRuleValid = GetInterestRule(DateTime.ParseExact(nextBreakDate, "yyyyMMdd", null));

                    days = (DateTime.ParseExact(nextBreakDate, "yyyyMMdd", null) -
                            DateTime.ParseExact(currentBreakDate, "yyyyMMdd", null)).Days + 1;

                    totalInterestValue += days * filteredTransactions[0].Balance * (nextRuleValid == null ? firstRule.Rate : nextRuleValid.Rate);
                    currentBreakDate = nextBreakDate;
                }
            }
            else
            {
                if (firstRule.RuleId == secondRule.RuleId)
                {
                    days = (DateTime.ParseExact(filteredTransactions[1].Date, "yyyyMMdd", null) -
                        DateTime.ParseExact(filteredTransactions[0].Date, "yyyyMMdd", null)).Days;
                    var rate = firstRule.Rate;
                    totalInterestValue += days * rate * filteredTransactions[0].Balance;
                }
                else
                {
                    var days1 = (DateTime.ParseExact(firsRuleValid.Date, "yyyyMMdd", null) -
                        DateTime.ParseExact(filteredTransactions[0].Date, "yyyyMMdd", null)).Days;
                    var rate1 = firstRule.Rate;

                    var days2 = (DateTime.ParseExact(filteredTransactions[1].Date, "yyyyMMdd", null) -
                        DateTime.ParseExact(firsRuleValid.Date, "yyyyMMdd", null)).Days;
                    var rate2 = secondRule.Rate;

                    totalInterestValue += (days1 * rate1 + days2 * rate2) * filteredTransactions[0].Balance;

                }
            }

            return totalInterestValue / 100;
        }
    }
}
