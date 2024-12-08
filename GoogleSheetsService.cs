using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace NTCBank
{
    internal static class GoogleSheetsService
    {
        private static readonly string ApplicationName = "NTCBank";
        private static readonly string SpreadsheetId = "1FzTzsWgELcgu92NDD09ZitshEJIM2DPA0zcpL2cx6Jg";
        private static readonly string SheetName = "Sheet1";
        private static SheetsService _service;

        static GoogleSheetsService()
        {
            InitializeService();
        }

        private static void InitializeService()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("service_account.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
            }

            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        /// <summary>
        /// Finds the row number of the given account in the sheet by AccountNumber.
        /// If not found, returns -1.
        /// </summary>
        private static int FindAccountRowInSheet(long accountNumber)
        {
            var range = $"{SheetName}!A2:A";
            var request = _service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;

            if (values == null || values.Count == 0)
                return -1;

            for (int i = 0; i < values.Count; i++)
            {
                var row = values[i];
                if (row.Count > 0 && long.TryParse(row[0].ToString(), out long existingAccNum))
                {
                    if (existingAccNum == accountNumber)
                    {
                        return i + 2; // +2 because we're starting from A2
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Updates the Google Sheet with the latest account details.
        /// Columns: A:AccountNumber, B:AccountName, C:CustomerName, D:CustomerType, E:Balance, F:CustomerEmail, G:CustomerPhone, H:CustomerAddress
        /// </summary>
        public static void UpdateAccountDetails(Accounts account)
        {
            if (account == null || account.AssociatedCustomer == null)
                return;

            var rowNumber = FindAccountRowInSheet(account.AccountNumber);
            var rowData = new List<object>()
            {
                account.AccountNumber,
                account.AccountName,
                account.AssociatedCustomer.CustomerName,
                account.AssociatedCustomer.CustomerType,
                account.Balance.ToString(),
                account.AssociatedCustomer.GetEmail(),
                account.AssociatedCustomer.GetPhone(),
                account.AssociatedCustomer.GetAddress()
            };

            var valueRange = new ValueRange { Values = new List<IList<object>> { rowData } };

            if (rowNumber == -1)
            {
                // Append new row
                var range = $"{SheetName}!A:H";
                var appendRequest = _service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
                appendRequest.Execute();
                Console.WriteLine("New account details appended to the sheet.");
            }
            else
            {
                // Update existing row
                var range = $"{SheetName}!A{rowNumber}:H{rowNumber}";
                var updateRequest = _service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                updateRequest.Execute();
                Console.WriteLine("Existing account details updated in the sheet.");
            }
        }

        /// <summary>
        /// Retrieves all accounts from the sheet.
        /// Columns expected: AccountNumber, AccountName, CustomerName, CustomerType, Balance, CustomerEmail, CustomerPhone, CustomerAddress
        /// </summary>
        public static List<Accounts> GetAllAccountsFromSheet()
        {
            var result = new List<Accounts>();
            var range = $"{SheetName}!A2:H"; // Assuming first row is headers
            var request = _service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;

            if (values == null || values.Count == 0)
            {
                Console.WriteLine("No account data found in the sheet.");
                return result;
            }

            foreach (var row in values)
            {
                if (row.Count < 8)
                    continue;

                // Extract and validate row data
                if (!long.TryParse(row[0].ToString(), out long accNumber))
                    continue;

                string accName = row[1].ToString();
                string customerName = row[2].ToString();
                string customerType = row[3].ToString();
                if (!decimal.TryParse(row[4].ToString(), out decimal balance))
                    continue;
                string customerEmail = row[5].ToString();
                string customerPhone = row[6].ToString();
                string customerAddress = row[7].ToString();

                // Create appropriate account type based on AccountName
                Accounts account;
                switch (accName.ToLower())
                {
                    case "savings":
                        account = new Accounts.Savings();
                        break;
                    case "checkings":
                        account = new Accounts.Checkings();
                        break;
                    case "trading":
                        account = new Accounts.Trading();
                        break;
                    default:
                        account = new Accounts();
                        break;
                }

                account.CreateAccount(accName, (int)accNumber);

                // Create the Customers object using retrieved details
                var customer = new Customers(customerName, customerType, customerEmail, customerPhone, customerAddress);
                account.AssociatedCustomer = customer;

                // Set the account balance by depositing the retrieved amount (0 initial + balance)
                if (balance > 0)
                {
                    account.DepositFunds(balance);
                }

                result.Add(account);
            }

            Console.WriteLine("Account data retrieved from sheet.");
            return result;
        }
    }
}
