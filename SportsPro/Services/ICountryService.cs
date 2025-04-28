using System.Collections.Generic;
using SportsPro.Models;

namespace SportsPro.Services
{
    public interface ICountryService
    {
        IEnumerable<Country> GetAllCountries();
    }
}