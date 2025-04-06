using System;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Registration
    {
        public int RegistrationId { get; set; }

        [Required]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}