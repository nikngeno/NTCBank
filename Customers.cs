using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NTCBank
{
    public class Customers
    {
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        private string CustomerEmail;
        private string CustomerPhone;
        private string CustomerAddress;

        public Customers(string name, string type, string email, string phone, string address)
        {
            CustomerName = name;
            CustomerType = type;
            CustomerEmail = email;
            CustomerPhone = phone;
            CustomerAddress = address;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail) || !newEmail.Contains("@"))
                throw new ArgumentException("Invalid email address.");
            CustomerEmail = newEmail;
        }

        public void UpdatePhone(string newPhone)
        {
            if (string.IsNullOrEmpty(newPhone) || newPhone.Length < 10)
                throw new ArgumentException("Invalid phone number.");
            CustomerPhone = newPhone;
        }

        public void UpdateAddress(string newAddress)
        {
            if (string.IsNullOrEmpty(newAddress))
                throw new ArgumentException("Address cannot be empty.");
            CustomerAddress = newAddress;
        }

        public void DisplayCustomerDetails()
        {
            Console.WriteLine("Customer Details:");
            Console.WriteLine($"Name: {CustomerName}");
            Console.WriteLine($"Type: {CustomerType}");
            Console.WriteLine($"Email: {CustomerEmail}");
            Console.WriteLine($"Phone: {CustomerPhone}");
            Console.WriteLine($"Address: {CustomerAddress}");
        }

        public string GetEmail()
        {
            return CustomerEmail;
        }

        public string GetPhone()
        {
            return CustomerPhone;
        }

        public string GetAddress()
        {
            return CustomerAddress;
        }
    }
}
