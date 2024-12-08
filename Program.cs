using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NTCBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            Console.WriteLine("Welcome to the NTCBank Management System!");
            List<Accounts> allAccounts = new List<Accounts>();

            while (running)
            {
                Console.WriteLine("\nPlease select an action:");
                Console.WriteLine("1. Open an account");
                Console.WriteLine("2. Make a deposit");
                Console.WriteLine("3. Make a withdrawal");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine()?.Trim();
                switch (choice)
                {
                    case "1":
                        OpenAccountWorkflow(allAccounts);
                        break;
                    case "2":
                        DepositWorkflow(allAccounts);
                        break;
                    case "3":
                        WithdrawalWorkflow(allAccounts);
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Thank you for using the Bank Management System. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }

        private static void OpenAccountWorkflow(List<Accounts> allAccounts)
        {
            Console.WriteLine("\n=== Open Account ===");
            Console.Write("Enter Customer Name: ");
            string customerName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(customerName))
            {
                Console.WriteLine("Invalid customer name. Returning to main menu.");
                return;
            }

            Console.Write("Enter Customer Address: ");
            string customerAddress = Console.ReadLine()?.Trim();

            Console.Write("Enter Customer Phone: ");
            string customerPhone = Console.ReadLine()?.Trim();

            var customer = new Customer(customerName, customerAddress, customerPhone);

            Console.WriteLine("\nSelect the type of account to open:");
            Console.WriteLine("1. Savings");
            Console.WriteLine("2. Checkings");
            Console.WriteLine("3. Trading");
            Console.Write("Enter your choice: ");
            string accountChoice = Console.ReadLine()?.Trim();

            Accounts account;
            switch (accountChoice)
            {
                case "1":
                    account = new Accounts.Savings { AccountName = "Savings" };
                    break;
                case "2":
                    account = new Accounts.Checkings { AccountName = "Checkings" };
                    break;
                case "3":
                    account = new Accounts.Trading { AccountName = "Trading" };
                    break;
                default:
                    Console.WriteLine("Invalid input. Returning to main menu.");
                    return;
            }

            long generatedNumber = DateTime.Now.Ticks;
            int newAccountNumber = (int)(generatedNumber % 1000000);
            account.CreateAccount(account.AccountName, newAccountNumber);
            account.AssociatedCustomer = customer;

            Console.Write("Enter initial deposit amount: ");
            string amountInput = Console.ReadLine()?.Trim();
            if (!decimal.TryParse(amountInput, out decimal initialDeposit) || initialDeposit < 0)
            {
                Console.WriteLine("Invalid amount. Returning to main menu.");
                return;
            }
            account.DepositFunds(initialDeposit);
            account.GetAccountDetails();
            Console.WriteLine($"Customer: {account.AssociatedCustomer.GetCustomerDetails()}");

            // Update Google Sheets with new account/customer details
            GoogleSheetsService.UpdateAccountDetails(account);

            allAccounts.Add(account);
        }

        private static void DepositWorkflow(List<Accounts> allAccounts)
        {
            if (allAccounts.Count == 0)
            {
                Console.WriteLine("No accounts found. Please open an account first.");
                return;
            }

            Console.Write("\nEnter the account number for deposit: ");
            string accNumberInput = Console.ReadLine()?.Trim();
            if (!long.TryParse(accNumberInput, out long accNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            var account = allAccounts.Find(a => a.AccountNumber == accNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.Write("Enter deposit amount: ");
            string amountInput = Console.ReadLine()?.Trim();
            if (!decimal.TryParse(amountInput, out decimal depositAmount) || depositAmount <= 0)
            {
                Console.WriteLine("Invalid deposit amount.");
                return;
            }

            account.DepositFunds(depositAmount);
            // Update Google Sheets with new balance
            GoogleSheetsService.UpdateAccountDetails(account);
        }

        private static void WithdrawalWorkflow(List<Accounts> allAccounts)
        {
            if (allAccounts.Count == 0)
            {
                Console.WriteLine("No accounts found. Please open an account first.");
                return;
            }

            Console.Write("\nEnter the account number for withdrawal: ");
            string accNumberInput = Console.ReadLine()?.Trim();
            if (!long.TryParse(accNumberInput, out long accNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            var account = allAccounts.Find(a => a.AccountNumber == accNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.Write("Enter withdrawal amount: ");
            string amountInput = Console.ReadLine()?.Trim();
            if (!decimal.TryParse(amountInput, out decimal withdrawalAmount) || withdrawalAmount <= 0)
            {
                Console.WriteLine("Invalid withdrawal amount.");
                return;
            }

            account.WithdrawFunds(withdrawalAmount);
            // Update Google Sheets with new balance
            GoogleSheetsService.UpdateAccountDetails(account);
        }
    }
}