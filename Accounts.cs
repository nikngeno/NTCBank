using System;

namespace NTCBank
{
    internal class Accounts
    {
        private string _AccountName;
        public string AccountName
        {
            set { _AccountName = value; }
            get { return _AccountName; }
        }

        private long _AccountNumber;
        public long AccountNumber
        {
            set { _AccountNumber = value; }
            get { return _AccountNumber; }
        }

        private decimal _Balance;
        public decimal Balance
        {
            get { return _Balance; }
        }

        public Customers AssociatedCustomer { get; set; }

        public Accounts()
        {
            AccountNumber = 0;
            AccountName = string.Empty;
        }

        public void CreateAccount(string accountName, int accountNumber)
        {
            AccountName = accountName;
            AccountNumber = accountNumber;
        }

        public void GetAccountDetails()
        {
            Console.WriteLine($"Account Name: {AccountName}, Account Number: {AccountNumber}, Balance: {Balance}");
        }

        public void DepositFunds(decimal amount)
        {
            if (amount > 0)
            {
                _Balance += amount;
                Console.WriteLine($"{amount} deposited successfully. New balance: {Balance}");
            }
            else
            {
                Console.WriteLine("Invalid deposit amount.");
            }
        }

        public void WithdrawFunds(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Invalid withdrawal amount.");
                return;
            }

            if (amount <= _Balance)
            {
                _Balance -= amount;
                Console.WriteLine($"{amount} withdrawn successfully. Remaining balance: {Balance}");
            }
            else
            {
                Console.WriteLine("Insufficient funds.");
            }
        }

        public void GetBalance()
        {
            Console.WriteLine($"Current Balance: {Balance}");
        }

        public class Savings : Accounts
        {
        }

        public class Checkings : Accounts
        {
        }

        public class Trading : Accounts
        {
        }
    }
}
