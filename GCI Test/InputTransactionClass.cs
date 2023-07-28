using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCI_Test
{
    public static class InputTransactionClass
    {
        public static void InputTransactions()
        {

            List<InterestRule> interestRules = new List<InterestRule>();

            Console.WriteLine("Please enter transaction details in <Date>|<Account>|<Type>|<Amount> format (or enter blank to go back to the main menu):");
            Console.Write("> ");
            string input = Console.ReadLine().Trim();


            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            string[] parts = input.Split('|');
            string account = parts[1].Trim();
            char type = char.ToUpper(parts[2].Trim()[0]);

            if (parts.Length != 4)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                InputTransactions();
                return;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                InputTransactions();


                if (type != 'D' && type != 'W')
                {
                    Console.WriteLine("Invalid transaction type. Please use 'D' for deposit or 'W' for withdrawal.");
                    InputTransactions();
                    return;
                }
            }


            if (!decimal.TryParse(parts[3], out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive numeric value.");
                InputTransactions();
                return;
            }

            var currentTransactions = ExistingTransacionList();

            if (!currentTransactions.Any(t => t.Account == account)) //checking if account already exist or not and the first transaction is W or nor
            {
                if (type == 'W')
                {
                    Console.WriteLine("The first transaction for an account should not be a withdrawal.");
                    InputTransactions();
                    return;
                }
                currentTransactions = new List<TransactionList>();
            }


            //Checking if the amount have zero balance

            decimal currentBalance = currentTransactions.Where(t => t.Account == account).Sum(t => (t.Type == 'D') ? t.Amount : -t.Amount);
            if (currentBalance + ((type == 'D') ? amount : -amount) < 0)
            {
                Console.WriteLine("The transaction cannot be completed. The account balance would be negative.");
                InputTransactions();
                return;
            }


            //Generating Transaction Id
            var currentTranctionId = (currentTransactions.Count(item => item.TransactionId.StartsWith(parts[0])) + 1).ToString("D2");

            TransactionList initialTransaction = new TransactionList
            {
                Date = parts[0],
                Account = account,
                Type = type,
                Amount = amount,
                TransactionId = parts[0] + '-' + currentTranctionId,
            };

            currentTransactions.Add(initialTransaction);

            Console.WriteLine("Transaction added successfully.");


            DisplayTransaction(currentTransactions);

        }

        static List<TransactionList> ExistingTransacionList()
        {
            List<TransactionList> transactionList = new List<TransactionList>
            {
                new TransactionList
                    {
                        Date = "20230505",TransactionId = "20230505-01",Type = 'D',Amount = 100.00m,Account = "AC001"
                    },
                new TransactionList
                    {
                        Date = "20230601",TransactionId = "20230601-01",Type = 'D',Amount = 150.00m,Account = "AC001"
                    },
                new TransactionList
                    {
                        Date = "20230626",TransactionId = "20230626-01",Type = 'W',Amount = 20.00m,Account = "AC001"
                    },
                new TransactionList
                    {
                        Date = "20230626",TransactionId = "20230626-02",Type = 'W',Amount = 100.00m,Account = "AC001"
                    }
            };

            return transactionList;
        }

        static void DisplayTransaction(List<TransactionList> transactionLists)
        {
            Console.WriteLine();
            Console.WriteLine($"Account: " + transactionLists[0].Account);
            Console.WriteLine("Date     | Txn Id      |  Type  |  Amount  ");
            foreach (var item in transactionLists)
            {
                Console.WriteLine(item.Date + " | " + item.TransactionId + " |    " + item.Type + "   | " + item.Amount);
            }
        }

    }
}
