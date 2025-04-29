using System.Collections.Generic;
using System.Linq;
using SportsPro.Models;
using SportsPro.Models.Data;

namespace SportsPro.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;

        public CustomerService(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _repository.List(new QueryOptions<Customer>()).ToList();
        }
    }
}