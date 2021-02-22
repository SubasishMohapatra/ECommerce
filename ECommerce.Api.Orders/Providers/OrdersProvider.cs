using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider:IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 1,
                    CustomerId=1,
                    OrderDate = DateTime.Now,
                    Total = 1000,
                    Items = new List<OrderItem>(){ new OrderItem{ Id=1,
                    ProductId=1, Quantity=2, UnitPrice=25 },new OrderItem{ Id=2,
                    ProductId=2,  Quantity=3, UnitPrice=26 } }
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 2,
                    CustomerId = 2,
                    OrderDate = DateTime.Now.AddDays(-7),
                    Total = 500,
                    Items = new List<OrderItem>(){ new OrderItem{ Id=3,
                    ProductId=1, Quantity=20, UnitPrice=250 },new OrderItem{ Id=4,
                    ProductId=2, Quantity=11, UnitPrice=260 } }
                });
                dbContext.SaveChangesAsync();
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                var orders = await dbContext.Orders.Include(x=>x.Items).ToListAsync();
                if(orders != null && orders.Any())
                {
                    var result=mapper.Map<List<Db.Order>, List<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }            
        }
        
        public async Task<(bool IsSucess, List<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int id)
        {
            try
            {
                var orders = dbContext.Orders.Include(x => x.Items).Where(p => p.Id == id);
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<List<Db.Order>, List<Models.Order>>(await orders.ToListAsync());
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
