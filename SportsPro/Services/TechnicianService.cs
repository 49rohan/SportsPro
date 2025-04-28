using System.Collections.Generic;
using System.Linq;
using SportsPro.Models;
using SportsPro.Models.Data;

namespace SportsPro.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly IRepository<Technician> _repository;

        public TechnicianService(IRepository<Technician> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Technician> GetAllTechnicians()
        {
            return _repository.List(new QueryOptions<Technician>()).ToList();
        }
    }
}