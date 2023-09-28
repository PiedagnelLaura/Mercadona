using Mercadona.Bogus;
using Mercadona.Context;
using Mercadona.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Mercadona.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MercadonaDbContext _dbContext;
       

        public HomeController(ILogger<HomeController> logger, MercadonaDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(AddData addData)
        {
            var products = await _dbContext.Products.ToListAsync();
            if (!products.Any())
            {
                addData.GenerateData(_dbContext);
                products = await _dbContext.Products.ToListAsync();

            }
                     
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}