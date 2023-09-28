using Mercadona.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mercadona.Context
{
    public class MercadonaDbContext :DbContext
    {

        public MercadonaDbContext(DbContextOptions<MercadonaDbContext> options) : base(options)
        {

        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Offer> Offers { get; set; }


       
    }
}
