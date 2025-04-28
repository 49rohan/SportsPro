using System.Collections.Generic;
using System.Linq;
using SportsPro.Models;
using SportsPro.Models.Data;

namespace SportsPro.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _repository.List(new QueryOptions<Product>()).ToList();
        }
    }
}
