using Mercadona.Bogus;
using Mercadona.Context;
using Mercadona.Controllers;
using Mercadona.Models;
using Mercadona.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MercadonaMSTest
{
    [TestClass]
    public class OfferTest
    {
        [TestMethod]
        public async Task PromotionIsAppliedWhenValidAsync()
        {
            // Arrange
            // On utilise une BDD in memory pour les tests
            var dbContextOptions = new DbContextOptionsBuilder<MercadonaDbContext>()
                .UseInMemoryDatabase(databaseName: "MercadonaTest")
                .Options;

            using (var dbContext = new MercadonaDbContext(dbContextOptions))
            {
                // On créé un produit avec une promotion valide
                Product product = new Product
                {
                    Name = "Produit Test",
                    Price = 100,
                    Description = "Description",
                    Picture = "https://picsum.photos/640/480/?image=613",
                    Offer = new Offer
                    {
                        StartDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Now.AddDays(1),
                        Discount = 20
                    }
                };

                dbContext.Products.Add(product);
                dbContext.SaveChanges();

                // Vérification que la promotion est correctement appliquée
                decimal expectedPrice = Math.Round(product.Price - (product.Price * product.Offer.Discount / 100), 2);

                // On fait appel au controller et au viewModel
                var controller = new HomeController(null, dbContext);
                var addData = new AddData();

                // Act
                var result = await controller.Index(addData) as ViewResult;
                var viewModel = result?.Model as HomeViewModel;

                // Assert
                if (viewModel != null && viewModel.NewPrices.Any())
                {
                    Assert.AreEqual(expectedPrice, viewModel.NewPrices[product.Id]);
                }
                else
                {
                    Assert.Fail("NewPrices is nul");
                }
            }
        }
    }
}