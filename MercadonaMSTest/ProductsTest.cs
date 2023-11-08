using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercadona.Controllers;
using Mercadona.Context;
using Mercadona.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Entity;

namespace MercadonaMSTest
{
    [TestClass]
    public class ProductsTest
    {
        private DbContextOptions<MercadonaDbContext> _options;

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<MercadonaDbContext>()
                .UseInMemoryDatabase(databaseName: "MercadonnaTest")
                .Options;
        }

        [TestMethod]
        public async Task GetAllProducts()
        {
            using (var dbContext = new MercadonaDbContext(_options))
            {
                // Arrange
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

        [TestMethod]
        public async Task CreateOneProduct()
        {
            using (var dbContext = new MercadonaDbContext(_options))
            {
                // Arrange
                var controller = new ProductsController(dbContext);
                Product newProduct = new Product
                {
                    Id = 56,
                    Name = "NewProduct",
                    Description = "Description of new product",
                    Picture = "https://picsum.photos/640/480/?image=613",
                    Price = 150,
                    Category = new Category { Name = "Test" }
                };

                // Act
                var result = await controller.Create(newProduct);

                // Assert
                var createdProduct = dbContext.Products.FirstOrDefault(p => p.Id == newProduct.Id);
                Assert.AreEqual(newProduct.Name, createdProduct?.Name);
            }
        }

        [TestMethod]
        public async Task AddOfferInProductPage()
        {
            using (var dbContext = new MercadonaDbContext(_options))
            {
                // Arrange
                List<Product> productsList = GetTestProducts();

                dbContext.Products.AddRange(productsList);

                Offer offer = new Offer
                {
                    Id = 1,
                    Discount = 50,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1)
                };
                dbContext.Offers.Add(offer);
                dbContext.SaveChanges();
                Product model = new Product
                {
                    Id = productsList[0].Id,
                    OfferId = offer.Id
                };

                var controller = new ProductsController(dbContext);

                // Act
                var result = await controller.AddOffer(model);

                // Assert
                var updatedProduct = dbContext.Products.FirstOrDefault(p => p.Id == model.Id);
                Assert.AreEqual(model.Id, updatedProduct?.OfferId);
            }
        }

        private List<Product> GetTestProducts()
        {
            List<Product> testProducts = new List<Product>
            {
                new Product 
                { 
                    Name = "Product1", 
                    Description = "Description1", 
                    Picture = "https://picsum.photos/640/480/?image=613", 
                    Price = 15 , 
                    Category = new Category
                    {
                        Name= "Test1"
                    } 
                },
                new Product 
                { 
                    Name = "Product2", 
                    Description = "Description2", 
                    Picture = "https://picsum.photos/640/480/?image=613", 
                    Price = 510 , 
                    Category = new Category
                    {
                        Name= "Test2"
                    }
                },
                new Product 
                { 
                    Name = "Product3", 
                    Description = "Description3", 
                    Picture = "https://picsum.photos/640/480/?image=613", 
                    Price = 16 , 
                    Category = new Category 
                    { 
                        Name = "Test3" 
                    }
                }
            };

            return testProducts;
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var dbContext = new MercadonaDbContext(_options))
            {
                dbContext.Database.EnsureDeleted();
            }
        }
    }
}