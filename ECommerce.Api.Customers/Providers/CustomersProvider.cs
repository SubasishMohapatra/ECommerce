using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider:ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer()
                {
                    Id = 1,
                    Name="Ram",
                    Address="India"
                });
                dbContext.Customers.Add(new Db.Customer()
                {
                    Id = 2,
                    Name = "John",
                    Address = "USA"
                });
                dbContext.SaveChangesAsync();
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if(customers != null && customers.Any())
                {
                    var result=mapper.Map<List<Db.Customer>, List<Models.Customer>>(customers);
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
        
        public async Task<(bool IsSucess, List<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync(int id)
        {
            try
            {
                var customers = dbContext.Customers.Where(x=>x.Id==id);
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<List<Db.Customer>, List<Models.Customer>>(await customers.ToListAsync());
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
