using ECommerce.Api.Customers.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSucess, IEnumerable<Customers.Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSucess, List<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync(int id);
    }
}
