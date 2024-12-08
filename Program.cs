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
                        OpenAccountWorkflow();
                        break;
                    case "2":
                        DepositWorkflow();
                        break;
                    case "3":
                        WithdrawalWorkflow();
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

        private static void OpenAccountWorkflow()
        {
            Console.WriteLine("\n=== Open Account ===");
            Console.WriteLine("This feature will be implemented in subsequent iterations.");
        }

        private static void DepositWorkflow()
        {
            Console.WriteLine("\n=== Make a Deposit ===");
            Console.WriteLine("This feature will be implemented in subsequent iterations.");
        }

        private static void WithdrawalWorkflow()
        {
            Console.WriteLine("\n=== Make a Withdrawal ===");
            Console.WriteLine("This feature will be implemented in subsequent iterations.");
        }
    }
}
