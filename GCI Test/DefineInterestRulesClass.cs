using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCI_Test
{
    public static class DefineInterestRulesClass
    {

        public static void DefineInterestRules()
        {
            Console.WriteLine("Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format (or enter blank to go back to the main menu):");
            Console.Write("> ");
            string input = Console.ReadLine().Trim();

            var interestRules = SharedClass.InterestRulesList();

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            string[] parts = input.Split('|');

            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                DefineInterestRules();
                return;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                DefineInterestRules();
                return;
            }

            string ruleId = parts[1].Trim();

            if (!decimal.TryParse(parts[2], out decimal rate) || rate <= 0 || rate >= 100)
            {
                Console.WriteLine("Invalid interest rate. Please enter a rate between 0 and 100.");
                DefineInterestRules();
                return;
            }

            // If there's any existing rule on the same day, remove it
            interestRules.RemoveAll(r => r.Date == parts[0]);

            InterestRule newRule = new InterestRule
            {
                Date = parts[0],
                RuleId = ruleId,
                Rate = rate
            };

            interestRules.Add(newRule);

            Console.WriteLine("Interest rule added successfully.");
            DisplayInsterest(interestRules);
        }

        static void DisplayInsterest(List<InterestRule> interestRule)
        {
            Console.WriteLine();
            Console.WriteLine($"Interest Rules");
            Console.WriteLine("Date     | RuleId |  Rate (%)");
            foreach (var item in interestRule)
            {
                Console.WriteLine(item.Date + " | " + item.RuleId + " |    " + item.Rate);
            }
        }
    }
}
