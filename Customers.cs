using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NTCBank
{
    internal class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Customer(string name, string address, string phone)
        {
            Name = name;
            Address = address;
            Phone = phone;
        }

        public string GetCustomerDetails()
        {
            return $"Name: {Name}, Address: {Address}, Phone: {Phone}";
        }
    }
}
