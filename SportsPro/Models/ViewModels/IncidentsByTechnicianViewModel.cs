using System.Collections.Generic;

namespace SportsPro.Models.ViewModels
{
    public class IncidentsByTechnicianViewModel
    {
        public Technician Technician { get; set; }
        public List<Incident> Incidents { get; set; }
    }
}
