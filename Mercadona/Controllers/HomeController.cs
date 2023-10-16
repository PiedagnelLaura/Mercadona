using Mercadona.Bogus;
using Mercadona.Context;
using Mercadona.Models;
using Mercadona.ViewModel;
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
            var viewModel = new HomeViewModel();
            viewModel.Products = await _dbContext.Products.ToListAsync();

            if (!viewModel.Products.Any())
            {
                addData.GenerateData(_dbContext);
                viewModel.Products = await _dbContext.Products.ToListAsync();
            }

            viewModel.NewPrices = new Dictionary<int, decimal>();

            foreach (var product in viewModel.Products)
            {
                if (product.Offer != null && product.Offer.StartDate < DateTime.Now && product.Offer.EndDate > DateTime.Now)
                {
                    decimal newPrice = Math.Round(product.Price - (product.Price * product.Offer.Discount/100),2);
                    viewModel.NewPrices.Add(product.Id, newPrice);
                }
            }

            viewModel.Categories = await _dbContext.Categories.ToListAsync();

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}