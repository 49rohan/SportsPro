using System.Collections.Generic;
using SportsPro.Models;

namespace SportsPro.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
    }
}