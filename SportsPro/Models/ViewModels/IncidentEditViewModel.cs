using System.Collections.Generic;

namespace SportsPro.Models.ViewModels
{
    public class IncidentEditViewModel
    {
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<Technician> Technicians { get; set; }
        public Incident CurrentIncident { get; set; }
        public string OperationType { get; set; } // "Add" or "Edit"
    }
}
