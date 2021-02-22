using ECommerce.Api.Orders.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrdersProvider
    {
        Task<(bool IsSucess, IEnumerable<Orders.Models.Order> Orders, string ErrorMessage)> GetOrdersAsync();
        Task<(bool IsSucess, List<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int id);
    }
}
