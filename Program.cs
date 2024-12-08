using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NTCBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            Console.WriteLine("Welcome to the NTCBank Management System!");
            List<Accounts> accounts = new List<Accounts>();

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
                        OpenAccount(accounts);
                        break;
                    case "2":
                        MakeDeposit(accounts);
                        break;
                    case "3":
                        MakeWithdrawal(accounts);
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

        private static void OpenAccount(List<Accounts> accounts)
        {
            Console.WriteLine("\n=== Open Account ===");
            Console.Write("Enter Customer Name: ");
            string name = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Invalid customer name. Returning to main menu.");
                return;
            }

            Console.Write("Enter Customer Type (e.g. Individual, SME, Corporate): ");
            string type = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(type))
            {
                Console.WriteLine("Invalid customer type. Returning to main menu.");
                return;
            }

            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Console.WriteLine("Invalid email address. Returning to main menu.");
                return;
            }

            Console.Write("Enter Customer Phone: ");
            string phone = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(phone))
            {
                Console.WriteLine("Invalid phone number. Returning to main menu.");
                return;
            }

            Console.Write("Enter Customer Address: ");
            string address = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(address))
            {
                Console.WriteLine("Address cannot be empty. Returning to main menu.");
                return;
            }

            var customer = new Customers(name, type, email, phone, address);

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
            account.AssociatedCustomer.DisplayCustomerDetails();

            GoogleSheetsService.UpdateAccountDetails(account);
            accounts.Add(account);
        }

        private static void MakeDeposit(List<Accounts> accounts)
        {
            if (accounts.Count == 0)
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

            var account = accounts.Find(a => a.AccountNumber == accNumber);
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
            GoogleSheetsService.UpdateAccountDetails(account);
        }

        private static void MakeWithdrawal(List<Accounts> accounts)
        {
            if (accounts.Count == 0)
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

            var account = accounts.Find(a => a.AccountNumber == accNumber);
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
            GoogleSheetsService.UpdateAccountDetails(account);
        }
    }
}
