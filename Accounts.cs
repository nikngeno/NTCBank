using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NTCBank
{
    // similar to customers this would be something like savings or checkings or trading accounts
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
        //private string _AccountPassword;
        public Accounts()
        {
            this.AccountNumber = 0;
            this.AccountName = string.Empty;
        }
        public void CreateAccount(string AccountName, int AccountNumber)
        {

        }
        public void GetAccountDetails()
        {

        }
        public void CloseAccount()
        {

        }
        public void DepositFunds()
        {

        }
        public void WidthrawFunds()
        {

        }
        public class Savings : Accounts
        {
            public Savings()
            {
            }
            private static void SetInterest()
            {

            }

        }
        
        public class Checkings : Accounts
        {
            public Checkings()
            {

            }
            private static void SetOverDraft()
            {

            }
            private static void GetOverDraft()
            {

            }
        }

        public class Trading : Accounts
        {
            public Trading()
            {

            }
            private static void CalculateProfit()
            {

            }

        }
    }
}
