using GCI_Test;
using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Security.Principal;
using System.Transactions;


Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
ShowMainMenu();

void ShowMainMenu()
{
    Console.WriteLine();
    Console.WriteLine("[I]nput transactions");
    Console.WriteLine("[D]efine interest rules");
    Console.WriteLine("[P]rint statement");
    Console.WriteLine("[Q]uit");
    Console.Write("> ");
    string input = Console.ReadLine().Trim().ToLower();

    switch (input)
    {
        case "i":
            InputTransactionClass.InputTransactions();
            ShowMainMenu();
        break;
        case "d":
            DefineInterestRulesClass.DefineInterestRules();
            ShowMainMenu();
            break;
        case "p":
            PrintStatementClass.PrintStatement();
            ShowMainMenu();
            break;
        case "q":
            Quit();
            break;
        default:
            Console.WriteLine("Invalid input. Please try again.");
            ShowMainMenu();
        break;

    }
}


static void Quit()
{
    Console.WriteLine();
    Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\r\nHave a nice day!\r\n");
    Environment.Exit(0);
}
