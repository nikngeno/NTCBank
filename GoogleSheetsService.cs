using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            // Make sure you have saved your service account JSON to "service_account.json"
            // and that the service account email is shared with the sheet.
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
      
        private static int FindAccountRowInSheet(long accountNumber)
        {
            var range = $"{SheetName}!A2:A"; // We will look for account numbers in column A
            var request = _service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;

            if (values == null || values.Count == 0)
                return -1;

            // Values start from row 2 in the sheet (A2), so we need to add an offset
            for (int i = 0; i < values.Count; i++)
            {
                var row = values[i];
                if (row.Count > 0 && long.TryParse(row[0].ToString(), out long existingAccNum))
                {
                    if (existingAccNum == accountNumber)
                    {
                        // Row index in the sheet (1-based). Since we started from A2, we add 2 for header offset.
                        return i + 2;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Updates the Google Sheet with the latest account details.
        /// If the account does not exist, it appends a new row.
        /// If it does exist, it updates that existing row.
        /// </summary>
        public static void UpdateAccountDetails(Accounts account)
        {
            if (account == null || account.AssociatedCustomer == null)
                return;

            var rowNumber = FindAccountRowInSheet(account.AccountNumber);

            // Data structure: AccountNumber, AccountName, CustomerName, CustomerType, Balance
            var rowData = new List<object>()
            {
                account.AccountNumber,
                account.AccountName,
                account.AssociatedCustomer.CustomerName,
                account.AssociatedCustomer.CustomerType,
                account.Balance.ToString()
            };

            if (rowNumber == -1)
            {
                // Append new row
                var range = $"{SheetName}!A:E";
                var valueRange = new ValueRange { Values = new List<IList<object>> { rowData } };
                var appendRequest = _service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
                appendRequest.Execute();
                Console.WriteLine("New account details appended to the sheet.");
            }
            else
            {
                // Update existing row
                var range = $"{SheetName}!A{rowNumber}:E{rowNumber}";
                var valueRange = new ValueRange { Values = new List<IList<object>> { rowData } };
                var updateRequest = _service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                updateRequest.Execute();
                Console.WriteLine("Existing account details updated in the sheet.");
            }
        }

        /// <summary>
        /// Retrieves all accounts from the sheet and returns them as a list of Accounts.
        /// This method can be used if you want to pull existing data from the sheet.
        /// </summary>
        public static List<Accounts> GetAllAccountsFromSheet()
        {
            var result = new List<Accounts>();
            var range = $"{SheetName}!A2:E"; // Assuming first row is headers
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
                if (row.Count < 5)
                    continue;

                if (!long.TryParse(row[0].ToString(), out long accNumber))
                    continue;

                var accName = row[1].ToString();
                var customerName = row[2].ToString();
                var customerType = row[3].ToString();
                if (!decimal.TryParse(row[4].ToString(), out decimal balance))
                    continue;

                // Create the appropriate account type based on AccountName if needed
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
                // We don't have full customer info (like Email, Phone, Address) from sheet here.
                // You may want to store these in the sheet as well. For now, just create a stub:
                var customer = new Customers(customerName, customerType, "N/A", "N/A", "N/A");
                account.AssociatedCustomer = customer;

                // Reflect balance
                account.DepositFunds(balance);

                result.Add(account);
            }

            Console.WriteLine("Account data retrieved from sheet.");
            return result;
        }
    }
}
