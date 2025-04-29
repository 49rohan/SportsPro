using System.Collections.Generic;
using SportsPro.Models;

namespace SportsPro.Services
{
    public interface ITechnicianService
    {
        IEnumerable<Technician> GetAllTechnicians();
    }
}
