using System.Collections.Generic;

namespace SportsPro.Models.ViewModels
{
    public class IncidentManagerViewModel
    {
        public List<Incident> Incidents { get; set; }
        public string FilterType { get; set; } // "All", "Unassigned", "Open"
    }
}
