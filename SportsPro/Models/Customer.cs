using System.Collections.Generic;

namespace SportsPro.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public List<Registration> Registrations { get; set; }

       
        public string FullName => $"{FirstName} {LastName}";
    }
}
