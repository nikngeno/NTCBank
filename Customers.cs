using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NTCBank
{
    // class that will hold different type of customers - from cooperate, SME and indivudual account that would have
    //different methods applied to them
    public class Customers
    {
        public string CustomerName { get; set; }
        public string CustomerType {get; set;}
        private string CustomerEmail;
        private string CustomerPhone;
        private string CustomerAddress;

        //constructor for customer detail intailization
        public Customers(string name, string type, string email, string phone, string address)
        {
            CustomerName = name;
            CustomerType = type;
            CustomerEmail = email;
            CustomerPhone = phone;
            CustomerAddress = address;
        }
            
      //method to update email
        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail) || !newEmail.Contains("@"))
                throw new ArgumentException("Invalid email address.");
            CustomerEmail = newEmail;
        }
        // Method to update phone number
        public void UpdatePhone(string newPhone)
        {
            if (string.IsNullOrEmpty(newPhone) || newPhone.Length < 10)
                throw new ArgumentException("Invalid phone number.");
            CustomerPhone = newPhone;
        }

        // Method to update address
        public void UpdateAddress(string newAddress)
        {
            if (string.IsNullOrEmpty(newAddress))
                throw new ArgumentException("Address cannot be empty.");
            CustomerAddress = newAddress;
        }

        // Method to display customer details
        public void DisplayCustomerDetails()
        {
            Console.WriteLine("Customer Details:");
            Console.WriteLine($"Name: {CustomerName}");
            Console.WriteLine($"Type: {CustomerType}");
            Console.WriteLine($"Email: {CustomerEmail}");
            Console.WriteLine($"Phone: {CustomerPhone}");
            Console.WriteLine($"Address: {CustomerAddress}");
        }
    }
    }
}
