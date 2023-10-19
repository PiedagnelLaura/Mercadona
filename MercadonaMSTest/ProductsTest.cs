using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercadona.Controllers;
using Mercadona.Context;
using Mercadona.Models;
using Microsoft.AspNetCore.Mvc;


namespace MercadonaMSTest
{
    [TestClass]
    public class ProductsTest
    {
        [TestMethod]
        public async Task GetAllProducts()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<MercadonaDbContext>()
                .UseInMemoryDatabase(databaseName: "MercadonnaTest")
                .Options;

            using (var dbContext = new MercadonaDbContext(dbContextOptions))
            {
                List<Product> testProducts = GetTestProducts();
                
                dbContext.Products.AddRange(testProducts);
                dbContext.SaveChanges();

                var controller = new ProductsController(dbContext);

                // Act
                var result = await controller.Index() as ViewResult;

                // Assert
                var listProduct = result?.ViewData.Model as List<Product>;
                Assert.AreEqual(testProducts.Count, listProduct?.Count);
            }
        }

        private List<Product> GetTestProducts()
        {
            List<Product> testProducts = new List<Product>
            {
                new Product { Name = "Product1", Description = "Description1", Picture = "https://picsum.photos/640/480/?image=613", Price = 15 , Category = new Category{Name= "Test1"} },
                new Product { Name = "Product2", Description = "Description2", Picture = "https://picsum.photos/640/480/?image=613", Price = 510 , Category = new Category{Name= "Test2"}},
                new Product { Name = "Product3", Description = "Description3", Picture = "https://picsum.photos/640/480/?image=613", Price = 16 , Category = new Category { Name = "Test3" }}
            };

            return testProducts;
        }
    }
}