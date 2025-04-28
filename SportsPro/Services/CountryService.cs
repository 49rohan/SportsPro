using System.Collections.Generic;
using System.Linq;
using SportsPro.Models;
using SportsPro.Models.Data;

namespace SportsPro.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _repository;

        public CountryService(IRepository<Country> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _repository.List(new QueryOptions<Country>()).ToList();
        }
    }
}
