using Mercadona.Context;
using Mercadona.Controllers;
using Mercadona.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mercadona.Bogus
{
    public class AddData
    {
        

        public void GenerateData(MercadonaDbContext dbContext)
        {
            try
            {
                Random random = new Random();

                List<Category> categories = new List<Category>
                {
                    new Category { Name = "Électronique" },
                    new Category { Name = "Vêtements" },
                    new Category { Name = "Alimentation" },
                    new Category { Name = "Mobilier" },
                    new Category { Name = "Livres" }
                };

                List<Offer> offers = new List<Offer>();
                OfferFaker offerFaker = new OfferFaker();
                for (int i = 0; i < 20; i++)
                {
                    Random randomDiscount = new Random();

                    Offer fakeOffer = offerFaker.Generate();
                    Offer offer = new Offer
                    {
                        StartDate = fakeOffer.StartDate,
                        EndDate = fakeOffer.EndDate,
                        Discount = randomDiscount.Next(5, 80),
                    };
                    offers.Add(offer);

                }

                List<Product> products = new List<Product>();
                ProductFaker productFaker = new ProductFaker();
                for (int i = 0; i < 20; i++)
                {
                    int randomIndexCat = random.Next(0, categories.Count);

                    int randomIndexOffer = random.Next(0, offers.Count);

                    Product fakeProduct = productFaker.Generate();
                    Product product = new Product
                    {
                        Name = fakeProduct.Name,
                        Description = fakeProduct.Description,
                        Price = fakeProduct.Price,
                        Picture = fakeProduct.Picture,
                        Category = categories[randomIndexCat],
                    };
                    if (i%2 == 1)
                    {
                        product.Offer = offers[randomIndexOffer];
                    }
                    products.Add(product);
                }

                MercadonaUser user = new MercadonaUser
                {
                    Id = "firstUser",
                    UserName = "admin@admin.com",
                    NormalizedUserName = "ADMIN@ADMIN.COM",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAECZFnQT0SkR5sCmyqpx8kViJZ9GKPBh6BeVy5J9MlKyi6shn95oC+/ISIlrsshlyvg==",
                    SecurityStamp = "GYA3AGUEBQ5LKI3E53WETVQDAFRLXQP4",
                    ConcurrencyStamp = "8c5c873f-8c5a-40be-9185-a64280329458",
                    LockoutEnabled = true,
                };


                IdentityRole role = new IdentityRole
                {
                    Id = "firstRole",
                    Name = "Administrator",
                    NormalizedName = "Administrator"

                };

                IdentityUserRole<string> userRole = new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                };


                dbContext.Users.Add(user);
                dbContext.Roles.Add(role);
                dbContext.UserRoles.Add(userRole);
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
