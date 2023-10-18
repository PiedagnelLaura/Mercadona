namespace Mercadona.Tests
{

    using Xunit;
    using Microsoft.EntityFrameworkCore;
    using Mercadona.Controllers;
    using Mercadona.Models;
    using Mercadona.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mercadona.Bogus;
    using Microsoft.AspNetCore.Mvc;
    using Mercadona.Context;
    using Moq;
    using System.Threading.Tasks;

    public class OfferTest
    {

        [Fact]
        public async Task PromotionIsAppliedWhenValidAsync()
        {
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
                    Description= "Description",
                    Picture = "https://picsum.photos/640/480/?image=613",
                    Offer = new Offer
                    {
                        StartDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Now.AddDays(1),
                        Discount = 20                    }
                };

                dbContext.Products.Add(product);
                dbContext.SaveChanges();


                // On fait appel au controller et au viewModel
                var controller = new HomeController(null, dbContext);
                var addData = new AddData();
                var result = await controller.Index(addData) as ViewResult;

                var viewModel = result?.Model as HomeViewModel;

                // Vérification que la promotion est correctement appliquée
                decimal expectedPrice = Math.Round(product.Price - (product.Price * product.Offer.Discount / 100), 2);
                if (viewModel != null && viewModel.NewPrices.Any() ) 
                {
                    Assert.Equal(expectedPrice, viewModel.NewPrices[product.Id]);
                }
                else 
                {
                    Assert.Fail("NewPrices is nul");
                }
            }
        }
    }
}
