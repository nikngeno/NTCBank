# NTC Bank Management System

The NTC Bank Management System is a C# console application designed to manage a bank’s core functionalities, including account creation, deposits, and withdrawals. The system integrates with Google Sheets via the Google API, allowing for seamless updates and tracking of account details in real-time. Below are the primary classes and their responsibilities:

1. **Program Class**  
   Serves as the entry point of the application.  
   - Prompts the user for various actions:  
     - Create a new account  
     - Make a deposit  
     - Perform a withdrawal

2. **Customer Class**  
   Handles customer information.  
   - Stores and manages customer details (e.g., name, ID, etc.)

3. **Accounts Class**  
   Manages all account-related operations.  
   - Defines account attributes (e.g., account number, balance)  
   - Implements methods for creating accounts, depositing funds, and withdrawing funds

4. **GoogleSheetService Class**  
   Facilitates interaction with Google Sheets via the Google API.  
   - Requires a credentials file placed in the same directory as the application code  
   - Reads and writes data to the Google Sheet based on user input and system actions

5. **Cards Class** *(Work in Progress)*  
   Intended to manage credit card creation and information.  
   - Planned functionality to issue a credit card with a limit set to 20% of the user’s deposited funds

6. **InterestCalculators Class** *(Work in Progress)*  
   Designed to calculate interest for savings accounts and future credit card functionalities.  
   - Will increase a user’s cash balance periodically based on defined interest rates and conditions

This system is modular and designed for easy expansion and integration with additional banking features. Contributions and improvements are welcome.
