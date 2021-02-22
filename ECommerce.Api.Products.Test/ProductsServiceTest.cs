using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Test
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task  GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
            await CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var producstProvider = new ProductsProvider(dbContext, null, mapper);
            var result=await producstProvider.GetProductsAsync();
            Assert.True(result.IsSucess);
            Assert.True(result.Products.Any());
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
            await CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var result = await productsProvider.GetProductAsync(1);
            Assert.True(result.IsSucess);
            Assert.NotNull(result.Product);
            Assert.True(result.Product.Id==1);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task GetProductDoesntReturnProductUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
            await CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var result = await productsProvider.GetProductAsync(-1);
            Assert.False(result.IsSucess);
            Assert.Null(result.Product);
            Assert.NotNull(result.ErrorMessage);
        }

        private async Task CreateProducts(ProductsDbContext dbContext)
        {
            for(int i=1;i<=10;i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            await dbContext.SaveChangesAsync();
        }
    }
}