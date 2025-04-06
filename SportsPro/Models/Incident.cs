using System;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Incident
    {
        public int IncidentID { get; set; }

        [Required]
        public int CustomerID { get; set; }     // Foreign key
        public Customer Customer { get; set; }  // Navigation property

        [Required]
        public int ProductID { get; set; }     // Foreign key
        public Product Product { get; set; }   // Navigation property

       
        public int? TechnicianID { get; set; }  // Nullable foreign key
        public Technician Technician { get; set; }  // Navigation property

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateOpened { get; set; } = DateTime.Now;

        public DateTime? DateClosed { get; set; } = null;

        [Required]
        public string Status { get; set; } = "Open";
    }
}
