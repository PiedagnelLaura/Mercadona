using Mercadona.Context;
using Mercadona.Controllers;
using Mercadona.Models;
using Microsoft.EntityFrameworkCore;

namespace Mercadona.Bogus
{
    public class AddData
    {
        

        public void GenerateData(MercadonaDbContext dbContext)
        {
            try
            {

                //var userFaker = new UserFaker();
                //for (int i = 0; i < 5; i++)
                //{

                //    var fakePeople = userFaker.Generate();
                //    var user = new User { Name = fakePeople.Name, Password = "admin" };

                //    _dbContext.Users.Add(user);
                //}

                var random = new Random();

                List<Category> categories = new List<Category>
                {
                    new Category { Name = "Électronique" },
                    new Category { Name = "Vêtements" },
                    new Category { Name = "Alimentation" },
                    new Category { Name = "Mobilier" },
                    new Category { Name = "Livres" }
                };

                List<Offer> offers = new List<Offer>();
                var offerFaker = new OfferFaker();
                for (int i = 0; i < 20; i++)
                {
                    var randomDiscount = new Random();

                    var fakeOffer = offerFaker.Generate();
                    var offer = new Offer
                    {
                        StartDate = fakeOffer.StartDate,
                        EndDate = fakeOffer.EndDate,
                        Discount = randomDiscount.Next(5, 80),
                    };
                    offers.Add(offer);

                }

                List<Product> products = new List<Product>();
                var productFaker = new ProductFaker();
                for (int i = 0; i < 20; i++)
                {
                    int randomIndexCat = random.Next(0, categories.Count);

                    var randomIndexOffer = random.Next(0, offers.Count);

                    var fakeProduct = productFaker.Generate();
                    var product = new Product
                    {
                        Name = fakeProduct.Name,
                        Description = fakeProduct.Description,
                        Price = fakeProduct.Price,
                        Picture = fakeProduct.Picture,
                        Category = categories[randomIndexCat],
                        Offer = offers[randomIndexOffer],
                    };
                    products.Add(product);
                }



                dbContext.Categories.AddRange(categories);
                dbContext.Products.AddRange(products);
                dbContext.Offers.AddRange(offers);
                dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
            }
        }
    }
}
